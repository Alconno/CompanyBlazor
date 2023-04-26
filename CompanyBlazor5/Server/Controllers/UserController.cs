using CompanyBlazor.Shared.Models;
using CompanyBlazor5.Client.Services;
using CompanyBlazor5.Server.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using System.Threading.Tasks;
using System.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using CompanyBlazor5.Shared;
using Microsoft.AspNetCore.Authorization;
using System.Linq.Expressions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Diagnostics;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Components;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc.Routing;
using CompanyBlazor5.Server.Encryption;

namespace CompanyBlazor5.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly CompanyBlazorDbContext _db;
        private readonly IConfiguration _config;
        private IEncrypt _encrypt;

        public UserController(CompanyBlazorDbContext db, IConfiguration configuration, IEncrypt encrypt)
        {
            _db=db;  
            _config=configuration;   
            _encrypt=encrypt;
        }

        [HttpGet("googleAuth")]
        public async Task<IActionResult> GoogleAuth(string reqMadeFrom)
        {
            return Challenge(new AuthenticationProperties
            {
                RedirectUri = Url.Action(nameof(GoogleAuthCallback), "User", new { reqMadeFrom = reqMadeFrom })
            },GoogleDefaults.AuthenticationScheme);
        }

        [HttpGet("googleAuth-callback")]
        public async Task<IActionResult>GoogleAuthCallback(string reqMadeFrom)
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            var claims = result.Principal.Identities.FirstOrDefault()
                .Claims.Select(claim => new
                {
                    claim.Issuer,
                    claim.OriginalIssuer,
                    claim.Type,
                    claim.Value
                });
            Login obj = new Login { loginUserName=claims.ElementAt(1).Value, loginPassword=claims.ElementAt(0).Value };

            // Check if user should be registered or logged in depending on value of the method parameter
            if (await GetAsync(obj.loginUserName) == null && reqMadeFrom=="register")
            {
                HttpContext.Session.Remove("Token"); // remove token
                HttpContext.Session.Clear(); // clear session
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme); // remove cookie on registration

                return await Task.FromResult(Redirect($"/register?Name={obj.loginUserName}&Password={obj.loginPassword}"));
            }
            else if (await GetAsync(obj.loginUserName) != null && reqMadeFrom=="login")
            {
                Debug.WriteLine(obj.loginUserName);
                return await Task.FromResult(Redirect($"/account?Name={obj.loginUserName}&Password={obj.loginPassword}"));
            }

            return await Task.FromResult(Redirect("/account"));
        }

        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Remove("Token"); // remove token
            HttpContext.Session.Clear(); // clear session
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme); // remove cookie on registration
            
            return await Task.FromResult(Redirect("/account"));
        }
        
        [HttpPost("register")]
        public async Task<RegisterResult> Register([FromBody]RegisterAccount reg, CancellationToken cancel)
        {
            try
            {
                Login createdUser = new Login { loginUserName=reg.Name, loginPassword=_encrypt.Hash(reg.Password) };
                if (await GetAsync(createdUser.loginUserName) != null)
                    return new RegisterResult { Message="Username already used.", Success=false };
                await _db.AddAsync(createdUser);
                await _db.SaveChangesAsync();
                if(createdUser!= null)
                    return new RegisterResult { Message="Registration successful.", Success = true };
                return new RegisterResult { Message="Registration unsuccessful.", Success = false };
            }
            catch(Exception ex)
            {
                return new RegisterResult { Message=ex.Message, Success = false };
            }
        }

        [HttpPost("login")]
        public async Task<LoginResult> Login([FromBody]LoginAccount log, CancellationToken cancel)
        {
            try
            {
                Login loadedUser = new Login { loginUserName=log.Name, loginPassword=_encrypt.Hash(log.Password) };
                Login dbUser = await GetAsync(loadedUser.loginUserName);
                if (dbUser != null && dbUser.loginPassword==loadedUser.loginPassword)
                    return new LoginResult { JWT = CreateJWT(loadedUser).Result, Name=loadedUser.loginUserName };
                return new LoginResult { Message="User/password was incorrect" };
            }
            catch(Exception ex)
            {
                return new LoginResult { Message = ex.Message };
            }
        }

        public async Task<Login> GetAsync(string name)
        {
            var listWithMatchingUsernames = _db.Logins.Where(n => n.loginUserName == name).ToListAsync().Result;
            if (listWithMatchingUsernames.Count()>=1) return await Task.FromResult(listWithMatchingUsernames.ElementAt(0));
            return null;
        }
        private async Task<string> CreateJWT(Login login)
        {
            var token = BuildToken(_config["Jwt:Key"].ToString(), _config["Jwt:Issuer"].ToString(), login);
            // also add cookie auth
            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);
            identity.AddClaim(new Claim(ClaimTypes.Name, login.loginUserName));
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, token));
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                new AuthenticationProperties
                {
                    IsPersistent = true,
                    AllowRefresh = true,
                    ExpiresUtc = DateTime.UtcNow.AddDays(1)
                });
            return token;
        }

        private const double EXPIRY_DURATION_MINUTES = 30;
        public string BuildToken(string key, string issuer, Login user)
        {
            var claims = new[] {
            new Claim(ClaimTypes.Name, user.loginUserName),
            new Claim(ClaimTypes.NameIdentifier, user.loginPassword)
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            var tokenDescriptor = new JwtSecurityToken(issuer, issuer, claims,
                expires: DateTime.Now.AddMinutes(EXPIRY_DURATION_MINUTES), signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }
        public bool ValidateToken(string key, string issuer, string token)
        {
            var mySecret = Encoding.UTF8.GetBytes(key);
            var mySecurityKey = new SymmetricSecurityKey(mySecret);
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                tokenHandler.ValidateToken(token,
                new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = issuer,
                    ValidAudience = issuer,
                    IssuerSigningKey = mySecurityKey,
                }, out SecurityToken validatedToken);
            }
            catch
            {
                return false;
            }
            return true;
        }

    }
}

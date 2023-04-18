using CompanyBlazor.Shared.Models;
using CompanyBlazor5.Shared;
using System.Text;
using System.Text.Json;

namespace CompanyBlazor5.Client
{
    public class JwtAuthenticationStateProvider : AuthenticationStateProvider
    {
        private static AuthenticationState NotAuthenticatedState = new AuthenticationState(new System.Security.Claims.ClaimsPrincipal());
        private LoginUser _user;

        public string DisplayName => this._user?.DisplayName;

        public bool IsLoggedIn => this._user != null;

        public string Token => this._user.Jwt;

        public void Login(string Jwt)
        {
            var principal = JwtSerialize.Deserialize(Jwt);
            this._user = new LoginUser(principal.Identity.Name, Jwt, principal);
            this.NotifyAuthenticationStateChanged(Task.FromResult(GetState()));
        }

        public void Logout()
        {
            this._user = null;
            this.NotifyAuthenticationStateChanged(Task.FromResult(GetState()));
        }

        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            return Task.FromResult(GetState());
        }
        private AuthenticationState GetState()
        {
            if (this._user != null)
            {
                return new AuthenticationState(this._user.claimsPrincipal);
            }
            else
            {
                return NotAuthenticatedState;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CompanyBlazor5.Shared
{
    public class LoginAccount
    {
        public string Name { get; set; }    
        public string Password { get; set; }
        public void Clear() => Name = Password = null;
    }
    public class LoginResult
    {
        public string Message { get; set; }
        public string Name { get; set; }
        public string JWT { get; set; }
    }
    public class LoginUser
    {
        public string DisplayName { get; set; }
        public string Jwt { get; set; }
        public ClaimsPrincipal claimsPrincipal { get; set; }

        public LoginUser(string displayName, string jwt, ClaimsPrincipal claimsPrincipal)
        {
            DisplayName=displayName;
            Jwt=jwt;
            this.claimsPrincipal=claimsPrincipal;   
        }
    }
}

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
}

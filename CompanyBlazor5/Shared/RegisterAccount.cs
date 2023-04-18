using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyBlazor5.Shared
{
    public class RegisterAccount
    {
        public string Password { get; set; }
        public string Name { get; set; }
        public void Clear() => Password= Name  =null;
    }
    public class RegisterResult
    {
        public string Message { get; set; }
        public bool Success { get; set; }
    }
}

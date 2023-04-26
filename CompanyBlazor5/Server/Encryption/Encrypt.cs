using System.Text;

namespace CompanyBlazor5.Server.Encryption
{
	public class Encrypt : IEncrypt
	{
        public string Hash(string value)
        {
            return Convert.ToBase64String(System.Security.Cryptography.SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(value)));
        }
    }
}

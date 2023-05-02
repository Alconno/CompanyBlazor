using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Cryptography;
using System.Text;

namespace CompanyBlazor5.Server.Encryption
{
	public class Encrypt : IEncrypt
	{
        private const string AesEncryptionKey = "fUjXn2r5u8x/A?D(G+KaPdSgVkYp3s6v";
        private const int KeySize = 256;
        private const int BlockSize = 128;

        public string Hash(string value)
        {
            return Convert.ToBase64String(System.Security.Cryptography.SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(value)));
        }

        public string BurrowEncode(string s)
        {
            if (s == "") return s;
            var mat = Enumerable.Range(0, s.Length).Select((_, i) => s.Substring(s.Length-i, i) + s.Substring(0, s.Length-i)).OrderBy(e => e, StringComparer.Ordinal);
            Tuple<string,int>tuple = new Tuple<string, int>(String.Join("", mat.Select(e => e[e.Length-1])), mat.ToList().IndexOf(s));
            return $"{tuple.Item1}_{tuple.Item2}";  
        }

        public string BurrowDecode(string s)
        {
            if (s == "") return "";
            int lastIndex = s.LastIndexOf("_"), idx = 0;
            if (lastIndex >= 0 && lastIndex < s.Length - 1)
            {
                string numberString = s.Substring(lastIndex + 1); 
                if (int.TryParse(numberString, out int number))
                {
                    s = s.Substring(0, lastIndex);
                    idx = number;
                }
                else return "";
            }
            else return "";
            var mat = Enumerable.Range(0, s.Length).Select(_ => "");
            for (int it = 0; it<s.Length; it++) mat = mat.Select((e, i) => s[i]+e).OrderBy(e => e, StringComparer.Ordinal);
            return mat.ElementAt(idx);
        }
    }

}

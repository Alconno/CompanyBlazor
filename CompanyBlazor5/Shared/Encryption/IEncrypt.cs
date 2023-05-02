namespace CompanyBlazor5.Server.Encryption
{
	public interface IEncrypt
	{
        public string Hash(string value);
        public string BurrowEncode(string s);
        public string BurrowDecode(string s);
    }
}
namespace CompanyBlazor5.Server.Encryption
{
	public interface IEncrypt
	{
        public string Hash(string value);
    }
}
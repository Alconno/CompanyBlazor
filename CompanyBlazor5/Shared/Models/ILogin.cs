namespace CompanyBlazor.Shared.Models
{
    public interface ILogin
    {
        int loginNo { get; set; }
        string loginPassword { get; set; }
        string loginUserName { get; set; }
    }
}
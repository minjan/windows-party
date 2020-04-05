namespace VPNClient.Services
{
    public interface IUserService
    {
        string GetToken();

        void SetToken(string token);

        void ClearSession();
    }
}

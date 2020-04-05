namespace VPNClient.Repositories
{
    public interface IUserSessionRepositoty
    {
        string Get();

        void Add(string token);

        void Remove();
    }
}

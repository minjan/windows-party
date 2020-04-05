namespace VPNClient.Repositories
{
    public class UserSessionRepositoty : IUserSessionRepositoty
    {
        private protected string Token { get; set; }

        public string Get()
        {
            return Token;
        }

        public void Add(string token)
        {
            Token = token;
        }

        public void Remove()
        {
            Token = string.Empty;
        }
    }
}

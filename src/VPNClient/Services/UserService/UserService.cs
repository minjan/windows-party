using VPNClient.Repositories;

namespace VPNClient.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly IUserSessionRepositoty _repository;
        public UserService(IUserSessionRepositoty repositoty)
        {
            _repository = repositoty;
        }

        public void ClearSession()
        {
            _repository.Remove();
        }

        public string GetToken()
        {
            return _repository.Get();
        }

        public void SetToken(string token)
        {
            _repository.Add(token);
        }
    }
}

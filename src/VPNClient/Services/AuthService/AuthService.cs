using Splat;
using System.Threading.Tasks;
using VPNClient.Models;
using VPNClient.Repositories;

namespace VPNClient.Services
{
    public class AuthService : IAuthService, IEnableLogger
    {
        private readonly IPlaygroundApi _api;

        public AuthService(IPlaygroundApi api)
        {
            _api = api;
        }

        public async Task<UserSession> AuthenticateAsyc(string username, string password)
        {
            return await _api.Authenticate(username, password);
        }
    }
}
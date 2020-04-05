using System.Threading.Tasks;
using VPNClient.Models;

namespace VPNClient.Services
{
    public interface IAuthService
    {
        Task<UserSession> AuthenticateAsyc(string username, string password);
    }
}

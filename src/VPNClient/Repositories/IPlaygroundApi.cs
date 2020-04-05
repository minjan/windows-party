using Refit;
using System.Threading.Tasks;
using VPNClient.Models;

namespace VPNClient.Repositories
{
    public interface IPlaygroundApi
    {
        [Post("/tokens")]
        [Headers("Content-Type: application/json; charset=UTF-8")]
        Task<UserSession> Authenticate(string username, string password);

        [Get("/servers")]
        [Headers("Content-Type: application/json; charset=UTF-8")]
        Task<Server[]> GetServers([Header("Authorization")] string token);
    }
}

using System.Threading.Tasks;
using VPNClient.Models;

namespace VPNClient.Services
{
    public interface IServersDataService
    {
        Task<Server[]> GetServers(string token);
    }
}
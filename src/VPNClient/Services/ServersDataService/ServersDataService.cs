using Splat;
using System.Threading.Tasks;
using VPNClient.Models;
using VPNClient.Repositories;

namespace VPNClient.Services
{
    public class ServersDataService : IServersDataService, IEnableLogger
    {
        private readonly IPlaygroundApi _api;
        public ServersDataService(IPlaygroundApi api)
        {
            _api = api;
        }

        public async Task<Server[]> GetServers(string token)
        {
            return await _api.GetServers($"Bearer {token}");
        }
    }
}

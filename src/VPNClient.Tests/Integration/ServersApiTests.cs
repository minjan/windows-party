using System.Threading.Tasks;
using VPNClient.Models;
using VPNClient.Services;
using Xunit;

namespace VPNClient.Tests.Integration
{
    public class ServersApiTests : BaseApi
    {
        private readonly ServersDataService sut;
        public ServersApiTests()
        {
            sut = new ServersDataService(Api);
        }

        [Fact(Skip = "Explicit test - token required")]
        public void GetServers()
        {
            TestGetServersAsync().Wait();
        }

        private async Task TestGetServersAsync()
        {
            var actual = await sut.GetServers(Token);

            Assert.NotNull(actual);
            Assert.All(actual, u => Assert.IsType<Server>(u));
        }
    }
}

using Xunit;
using VPNClient.Services;
using System.Threading.Tasks;

namespace VPNClient.Tests.Integration
{
    public class ApiIntegrationTests : BaseApi
    {
        private readonly AuthService sut;

        public ApiIntegrationTests()
        {
            sut = new AuthService(Api);
        }

        [Fact(Skip = "Explicit test - login required")]
        public void Autenticate()
        {
            TestAutenticateAsync().Wait();
        }

        private async Task TestAutenticateAsync()
        {
            var userSession = await sut.AuthenticateAsyc(Username, Password);

            Assert.NotNull(userSession);
            Assert.Equal(Token, userSession.Token);
        }
    }
}

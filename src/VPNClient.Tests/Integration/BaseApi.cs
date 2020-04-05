using Refit;
using VPNClient.Repositories;

namespace VPNClient.Tests.Integration
{
    public class BaseApi
    {
        protected readonly string Username = "";
        protected readonly string Password = "";
        protected readonly string Token = "";
        private string ApiUrl => "";

        protected IPlaygroundApi Api => RestService.For<IPlaygroundApi>(ApiUrl);
    }
}

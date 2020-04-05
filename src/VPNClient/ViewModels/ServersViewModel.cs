using ReactiveUI;
using Splat;
using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using VPNClient.Models;
using VPNClient.Services;

namespace VPNClient.ViewModels
{
    public class ServersViewModel : ReactiveObject, IRoutableViewModel
    {
        private readonly IServersDataService _serversDataService;
        private readonly IUserService _userService;
        public IScreen HostScreen { get; }
        public ReactiveCommand<Unit, Unit> Logout { get; }
        public ReactiveCommand<Unit, Server[]> LoadServers { get; }
        private readonly ObservableAsPropertyHelper<Server[]> _servers;
        public Server[] Servers => _servers.Value;
        public string UrlPathSegment => "servers";

        private readonly ObservableAsPropertyHelper<bool> _isBusy;
        public bool IsBusy => _isBusy.Value;

        public ServersViewModel(IScreen hostScreen, IServersDataService serversService = null, IUserService userService = null)
        {
            _serversDataService = serversService ?? Locator.Current.GetService<IServersDataService>();
            _userService = userService ?? Locator.Current.GetService<IUserService>();
            HostScreen = hostScreen;
           
            LoadServers = ReactiveCommand.CreateFromTask(() => LoadServersAsync());

            _servers = LoadServers.Execute().ToProperty(this, x => x.Servers);

            LoadServers
                   .ThrownExceptions
                   .Subscribe(x => this.Log().Error($"{x.Message}\n{x.StackTrace}"));

            _isBusy = LoadServers.IsExecuting.ToProperty(this, x => x.IsBusy);

            Logout = ReactiveCommand.CreateFromTask(() => LogOut());
        }

        private async Task LogOut()
        {
            _userService.ClearSession();
            await HostScreen.Router.Navigate.Execute(new LoginViewModel(HostScreen)).Select(_ => Unit.Default);
        }

        private async Task<Server[]> LoadServersAsync()
        {
            var token = _userService.GetToken();
            return await _serversDataService.GetServers(token);
        }
    }
}

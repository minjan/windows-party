using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Splat;
using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using VPNClient.Services;

namespace VPNClient.ViewModels
{
    public class LoginViewModel : ReactiveObject, ILoginViewModel
    {
        public IScreen HostScreen { get; private set; }
        private readonly IAuthService _authService;
        private readonly IUserService _userService;

        public string UrlPathSegment
        {
            get { return "login"; }
        }
        [Reactive] public string UserName { get; set; }
        [Reactive] public string Password { get; set; }
        public ReactiveCommand<Unit, Unit> Login { get; }
        [Reactive] public string ErrorMessage { get; private set; }

        private readonly ObservableAsPropertyHelper<bool> _isBusy;
        public bool IsBusy => _isBusy.Value;

        public LoginViewModel(IScreen screen, IAuthService authService = null, IUserService userService = null)
        {
            _authService = authService ?? Locator.Current.GetService<IAuthService>();
            _userService = userService ?? Locator.Current.GetService<IUserService>();
            HostScreen = screen;

            var canLogin = this.WhenAnyValue(
                        x => x.UserName,
                        x => x.Password,
                        (username, password) =>
                            !string.IsNullOrWhiteSpace(username) &&
                            !string.IsNullOrWhiteSpace(password));

            Login = ReactiveCommand.CreateFromTask(_ => Authenticate(this.UserName, this.Password), canExecute: canLogin);

            Login
                .ThrownExceptions                
                .Subscribe(ex => HandleException(ex));

            _isBusy = Login.IsExecuting.ToProperty(this, x => x.IsBusy);
        }

        private void HandleException(Exception ex)
        {
            ErrorMessage = ex.Message switch
            {
                string s when s.Contains("401") => "Incorrect login credentials. Please try again",
                _ => "Unknown error. Please try again",
            };
            this.Log().Error($"{ex.Message}\n{ex.StackTrace}");
        }

        private async Task Authenticate(string userName, string password)
        {
            //await Task.Delay(1000); //Loading cog
            var userSession = await _authService.AuthenticateAsyc(userName, password);
            if (userSession != null)
            {
                _userService.SetToken(userSession.Token);
                await HostScreen.Router.Navigate.Execute(new ServersViewModel(HostScreen)).Select(_ => Unit.Default);
            }
        }
    }
}

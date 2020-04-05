using ReactiveUI;
using System.Reactive;

namespace VPNClient.ViewModels
{
    public interface ILoginViewModel : IRoutableViewModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        ReactiveCommand<Unit, Unit> Login { get; }
        public string ErrorMessage { get; }
        public bool IsBusy { get; }
    }
}

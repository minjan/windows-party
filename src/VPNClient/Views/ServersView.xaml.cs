using ReactiveUI;

namespace VPNClient.Views
{
    public partial class ServersView
    {
        public ServersView()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                d(this.OneWayBind(ViewModel, vm => vm.Servers, v => v.ServersListBox.ItemsSource));
                d(this.BindCommand(ViewModel, vm => vm.Logout, v=> v.Logout));
            });
        }
    }
}

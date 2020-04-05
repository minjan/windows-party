using ReactiveUI;
using System.Windows;
using System.Windows.Controls;
using VPNClient.ViewModels;

namespace VPNClient.Views
{
    public partial class LoginView : IViewFor<ILoginViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(ILoginViewModel), typeof(LoginView), new PropertyMetadata(null));

        public LoginView()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                d(this.WhenAnyValue(vm => vm.ViewModel).BindTo(this, v => v.DataContext));
                d(this.Bind(ViewModel, vm => vm.UserName, v => v.tbxUsername.Text));
                d(this.OneWayBind(ViewModel, vm => vm.ErrorMessage, v => v.Message.Text));
                d(this.OneWayBind(ViewModel, vm => vm.IsBusy, v => v.Spinner.Visibility, 
                    vmToViewConverterOverride: new BooleanToVisibilityTypeConverter()));
                d(this.BindCommand(ViewModel, vm => vm.Login, v => v.Login));
            });
        }

        public ILoginViewModel ViewModel
        {
            get => (ILoginViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (ILoginViewModel)value;
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordBox passwordBox = ((PasswordBox)sender);
            if (ViewModel != null)
                ViewModel.Password = passwordBox.Password;
        }
    }
}

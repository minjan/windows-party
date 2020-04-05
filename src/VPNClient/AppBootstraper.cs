using System;
using ReactiveUI;
using Refit;
using Splat;
using Splat.NLog;
using VPNClient.Repositories;
using VPNClient.Services;
using VPNClient.Services.UserService;
using VPNClient.ViewModels;
using VPNClient.Views;

namespace VPNClient
{
    public class AppBootstrapper : ReactiveObject, IScreen
    {
        public RoutingState Router { get; private set; }

        public AppBootstrapper(IMutableDependencyResolver dependencyResolver = null, RoutingState testRouter = null)
        {
            Router = testRouter ?? new RoutingState();
            dependencyResolver ??= Locator.CurrentMutable;

            // Bind 
            RegisterParts(dependencyResolver);

            //Logging
            Locator.CurrentMutable.UseNLogWithWrappingFullLogger();
            this.Log().Debug("Start NLog with wrapping full logger!");

            // Navigate to the opening page of the application
            Router
                .Navigate
                .Execute(new LoginViewModel(this))
                .Subscribe();
        }

        private void RegisterParts(IMutableDependencyResolver dependencyResolver)
        {
            dependencyResolver.RegisterConstant(this, typeof(IScreen));

            dependencyResolver.RegisterLazySingleton(() => RestService.For<IPlaygroundApi>("http://playground.tesonet.lt/v1/"));
            dependencyResolver.RegisterLazySingleton<IUserSessionRepositoty>(() => new UserSessionRepositoty());

            dependencyResolver.Register<IUserService>(() => new UserService(Locator.Current.GetService<IUserSessionRepositoty>()));
            dependencyResolver.Register<IServersDataService>(() => new ServersDataService(Locator.Current.GetService<IPlaygroundApi>()));
            dependencyResolver.Register<IAuthService>(() => new AuthService(Locator.Current.GetService<IPlaygroundApi>()));

            dependencyResolver.Register(() => new LoginView(), typeof(IViewFor<LoginViewModel>));
            dependencyResolver.Register(() => new ServersView(), typeof(IViewFor<ServersViewModel>));
        }
    }
}
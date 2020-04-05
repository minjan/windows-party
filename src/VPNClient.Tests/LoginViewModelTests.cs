using FluentAssertions;
using Microsoft.Reactive.Testing;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using ReactiveUI;
using ReactiveUI.Testing;
using System;
using VPNClient.Models;
using VPNClient.Services;
using VPNClient.ViewModels;
using Xunit;

namespace VPNClient.Tests
{
    public class LoginViewModelTests
    {
        [Fact]
        public void ShouldBeBusyWhenLoginIsStarted() => new TestScheduler().With(scheduler =>
        {
            // Use NSubstute to generate stubs and mocks.
            var screen = Substitute.For<IScreen>();
            var authService = Substitute.For<IAuthService>();
            var userService = Substitute.For<IUserService>();
            var model = new LoginViewModel(screen, authService, userService)
            {
                UserName = "user",
                Password = "password"
            };

            // A test needs to subscribe to a ReactiveCommand,
            // because the execution is lazy and won't trigger
            // with no subscribers.
            model.Login.Execute().Subscribe();

            // We advance our scheduler with two ticks.
            // On the first tick, IsExecuting emits a new value,
            // and the second tick invokes IsBusy.
            scheduler.AdvanceBy(2);
            model.IsBusy.Should().BeTrue();

            scheduler.AdvanceBy(2);
            model.IsBusy.Should().BeFalse();

            Assert.Null(model.ErrorMessage);
        });

        [Fact]
        public void ShouldThrowExeptionWhenAuthServiceThrowErrors() => new TestScheduler().With(scheduler =>
        {
            var screen = Substitute.For<IScreen>();
            var authService = Substitute.For<IAuthService>();
            var userService = Substitute.For<IUserService>();

            authService
                .When(_ => _.AuthenticateAsyc(Arg.Any<string>(), Arg.Any<string>()))
                .Throw(new Exception("401 Unauthorized"));

            var model = new LoginViewModel(screen, authService, userService)
            {
                UserName = "user",
                Password = "wrongpassword"
            };

            model.Login.Execute().Should().Throws(new Exception("401 Unauthorized"));
        });

        [Fact]
        public void ShouldLoginWithCorrectCredentials() => new TestScheduler().With(scheduler =>
        {
            var token = "token";
            var screen = Substitute.For<IScreen>();
            var authService = Substitute.For<IAuthService>();
            var userService = Substitute.For<IUserService>();

            authService
                .AuthenticateAsyc(Arg.Any<string>(), Arg.Any<string>())
                .Returns(new UserSession { Token = token });

            var model = new LoginViewModel(screen, authService, userService)
            {
                UserName = "user",
                Password = "password"
            };

            model.Login.Execute().Subscribe();

            Received.InOrder(() =>
            {
                authService.AuthenticateAsyc(Arg.Any<string>(), Arg.Any<string>());
                userService.SetToken(token);
            });
        });
    }
}

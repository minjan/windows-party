using FluentAssertions;
using Microsoft.Reactive.Testing;
using NSubstitute;
using ReactiveUI;
using ReactiveUI.Testing;
using System;
using VPNClient.Models;
using VPNClient.Services;
using VPNClient.ViewModels;
using Xunit;

namespace VPNClient.Tests
{
    public class ServersViewModelTests
    {
        [Fact]
        public void ShouldBeBusyWhenServerDataIsLoading() => new TestScheduler().With(scheduler =>
        {
            // Use NSubstute to generate stubs and mocks.
            var screen = Substitute.For<IScreen>();
            var serversDataService = Substitute.For<IServersDataService>();
            var userService = Substitute.For<IUserService>();
            var model = new ServersViewModel(screen, serversDataService, userService);

            // A test needs to subscribe to a ReactiveCommand,
            // because the execution is lazy and won't trigger
            // with no subscribers.
            model.LoadServers.Execute().Subscribe();

            // We advance our scheduler with two ticks.
            // On the first tick, IsExecuting emits a new value,
            // and the second tick invokes IsBusy.
            scheduler.AdvanceBy(2);
            model.IsBusy.Should().BeTrue();

            scheduler.AdvanceBy(4);
            model.IsBusy.Should().BeFalse();
        });

        [Fact]
        public void ShouldReturnCorrectData()
        {
            var screen = Substitute.For<IScreen>();
            var serversDataService = Substitute.For<IServersDataService>();
            var userService = Substitute.For<IUserService>();

            Server[] servers = new Server[] { 
                new Server { Name = "Lithuania", Distance = 50 }, 
                new Server { Name = "Latvia", Distance = 150 } };

            serversDataService
                .GetServers(Arg.Any<string>())
                .Returns(servers);

            var model = new ServersViewModel(screen, serversDataService, userService);

            model.LoadServers.Execute().Subscribe();

            model.Servers.Length.Should().Be(2);
        }
    }
}

// Licensed under the MIT License.
// Copyright (c) 2018-2020 the AppCore .NET project.

using System;
using System.Threading;
using System.Threading.Tasks;
using AppCore.Logging;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Xunit;

namespace AppCore.Hosting
{
    public class BackgroundServiceHostTests
    {
        [Fact]
        public async Task StartStartAllServices()
        {
            var service1 = Substitute.For<IBackgroundService>();
            var service2 = Substitute.For<IBackgroundService>();

            var host = new BackgroundServiceHost(
                new[] {service1, service2},
                Substitute.For<ILogger<BackgroundServiceHost>>());
            
            await host.StartAsync(CancellationToken.None);

            await service1.Received(1)
                          .StartAsync(Arg.Any<CancellationToken>());

            await service2.Received(1)
                          .StartAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task StartThrowsForAny()
        {
            var service1 = Substitute.For<IBackgroundService>();
            service1.StartAsync(Arg.Any<CancellationToken>())
                    .Throws(new InvalidOperationException());

            var service2 = Substitute.For<IBackgroundService>();

            var host = new BackgroundServiceHost(
                new[] {service1, service2},
                Substitute.For<ILogger<BackgroundServiceHost>>());

            await Assert.ThrowsAsync<InvalidOperationException>(
                async () => await host.StartAsync(CancellationToken.None));
        }

        [Fact]
        public async Task CancelsStartWhenAnyThrows()
        {
            var service1 = Substitute.For<IBackgroundService>();
            service1.StartAsync(Arg.Any<CancellationToken>())
                    .Returns(
                        async ci =>
                        {
                            await Task.Delay(500);
                            ci.ArgAt<CancellationToken>(0)
                              .IsCancellationRequested.Should()
                              .BeTrue();
                        });

            var service2 = Substitute.For<IBackgroundService>();
            service2.StartAsync(Arg.Any<CancellationToken>())
                    .Throws(new InvalidOperationException());

            var host = new BackgroundServiceHost(
                new[] {service1, service2},
                Substitute.For<ILogger<BackgroundServiceHost>>());

            await Assert.ThrowsAsync<InvalidOperationException>(
                async () => await host.StartAsync(CancellationToken.None));
        }

        [Fact]
        public async Task StopsRunningWhenAnyThrows()
        {
            var service1 = Substitute.For<IBackgroundService>();

            var service2 = Substitute.For<IBackgroundService>();
            service2.StartAsync(Arg.Any<CancellationToken>())
                    .Returns(
                        async ci =>
                        {
                            await Task.Delay(500);
                            throw new InvalidOperationException();
                        });

            var host = new BackgroundServiceHost(
                new[] {service1, service2},
                Substitute.For<ILogger<BackgroundServiceHost>>());

            await Assert.ThrowsAsync<InvalidOperationException>(
                async () => await host.StartAsync(CancellationToken.None));

            await service1.Received(1)
                          .StopAsync(Arg.Any<CancellationToken>());
        }


        [Fact]
        public async Task StopsAll()
        {
            var service1 = Substitute.For<IBackgroundService>();
            var service2 = Substitute.For<IBackgroundService>();

            var host = new BackgroundServiceHost(
                new[] {service1, service2},
                Substitute.For<ILogger<BackgroundServiceHost>>());
            
            await host.StopAsync(CancellationToken.None);

            await service1.Received(1)
                          .StopAsync(Arg.Any<CancellationToken>());

            await service2.Received(1)
                          .StopAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task StopThrowsForAny()
        {
            var service1 = Substitute.For<IBackgroundService>();
            service1.StopAsync(Arg.Any<CancellationToken>())
                    .Throws(new InvalidOperationException());

            var service2 = Substitute.For<IBackgroundService>();

            var host = new BackgroundServiceHost(
                new[] {service1, service2},
                Substitute.For<ILogger<BackgroundServiceHost>>());

            await Assert.ThrowsAsync<InvalidOperationException>(
                async () => await host.StopAsync(CancellationToken.None));
        }

        [Fact]
        public async Task CancelsStopWhenAnyThrows()
        {
            var service1 = Substitute.For<IBackgroundService>();
            service1.StopAsync(Arg.Any<CancellationToken>())
                    .Returns(
                        async ci =>
                        {
                            await Task.Delay(500);
                            ci.ArgAt<CancellationToken>(0)
                              .IsCancellationRequested.Should()
                              .BeTrue();
                        });

            var service2 = Substitute.For<IBackgroundService>();
            service2.StopAsync(Arg.Any<CancellationToken>())
                    .Throws(new InvalidOperationException());

            var host = new BackgroundServiceHost(
                new[] {service1, service2},
                Substitute.For<ILogger<BackgroundServiceHost>>());

            await Assert.ThrowsAsync<InvalidOperationException>(
                async () => await host.StopAsync(CancellationToken.None));
        }
    }
}

using System;
using System.Threading;
using System.Threading.Tasks;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Xunit;

namespace AppCore.Hosting
{
    public class BackgroundServiceHostTests
    {
        [Fact]
        public async Task StartsAllServices()
        {
            var service1 = Substitute.For<IBackgroundService>();
            var service2 = Substitute.For<IBackgroundService>();

            var host = new BackgroundServiceHost(new[] {service1, service2});
            
            await host.StartAsync(CancellationToken.None);

            await service1.Received(1)
                          .StartAsync(Arg.Any<CancellationToken>());

            await service2.Received(1)
                          .StartAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task ThrowsForAnyService()
        {
            var service1 = Substitute.For<IBackgroundService>();
            service1.StartAsync(Arg.Any<CancellationToken>())
                    .Throws(new InvalidOperationException());

            var host = Substitute.For<IBackgroundService>();

            var adapter = new BackgroundServiceHost(new[] {service1, host});

            await Assert.ThrowsAsync<InvalidOperationException>(
                async () => await adapter.StartAsync(CancellationToken.None));

            await host.Received(1)
                          .StartAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task StopsStartedServicesWhenAnyThrows()
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

            var host = new BackgroundServiceHost(new[] {service1, service2});

            await Assert.ThrowsAsync<InvalidOperationException>(
                async () => await host.StartAsync(CancellationToken.None));

            await service1.Received(1)
                          .StopAsync(Arg.Any<CancellationToken>());
        }


        [Fact]
        public async Task StopsAllServices()
        {
            var service1 = Substitute.For<IBackgroundService>();
            var service2 = Substitute.For<IBackgroundService>();

            var host = new BackgroundServiceHost(new[] {service1, service2});
            
            await host.StopAsync(CancellationToken.None);

            await service1.Received(1)
                          .StopAsync(Arg.Any<CancellationToken>());

            await service2.Received(1)
                          .StopAsync(Arg.Any<CancellationToken>());
        }
    }
}

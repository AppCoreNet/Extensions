using System.Threading;
using System.Threading.Tasks;
using AppCore.Logging;
using NSubstitute;
using Xunit;

namespace AppCore.Hosting.Microsoft.Extensions
{
    public class BackgroundServiceHostAdapterTests
    {
        [Fact]
        public async Task StartsAllBackgroundServices()
        {
            var service1 = Substitute.For<IBackgroundService>();
            var service2 = Substitute.For<IBackgroundService>();

            var adapter = new BackgroundServiceHostAdapter(
                new[] {service1, service2},
                Substitute.For<ILogger<BackgroundServiceHost>>());
            
            await adapter.StartAsync(CancellationToken.None);

            await service1.Received(1)
                          .StartAsync(Arg.Any<CancellationToken>());

            await service2.Received(1)
                          .StartAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task StopsAllBackgroundServices()
        {
            var service1 = Substitute.For<IBackgroundService>();
            var service2 = Substitute.For<IBackgroundService>();

            var adapter = new BackgroundServiceHostAdapter(
                new[] {service1, service2},
                Substitute.For<ILogger<BackgroundServiceHost>>());
            
            await adapter.StopAsync(CancellationToken.None);

            await service1.Received(1)
                          .StopAsync(Arg.Any<CancellationToken>());

            await service2.Received(1)
                          .StopAsync(Arg.Any<CancellationToken>());
        }
    }
}

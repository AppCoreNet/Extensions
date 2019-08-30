using System.Threading;
using System.Threading.Tasks;
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

            var adapter = new BackgroundServiceHostAdapter(new[] {service1, service2});
            
            var ct = new CancellationToken();
            await adapter.StartAsync(ct);

            await service1.Received(1)
                          .StartAsync(ct);

            await service2.Received(1)
                          .StartAsync(ct);
        }

        [Fact]
        public async Task StopsAllBackgroundServices()
        {
            var service1 = Substitute.For<IBackgroundService>();
            var service2 = Substitute.For<IBackgroundService>();

            var adapter = new BackgroundServiceHostAdapter(new[] {service1, service2});
            
            var ct = new CancellationToken();
            await adapter.StopAsync(ct);

            await service1.Received(1)
                          .StopAsync(ct);

            await service2.Received(1)
                          .StopAsync(ct);
        }
    }
}

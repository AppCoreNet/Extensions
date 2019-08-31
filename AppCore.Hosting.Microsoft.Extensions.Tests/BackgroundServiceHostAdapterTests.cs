using System.Threading;
using System.Threading.Tasks;
using NSubstitute;
using Xunit;

namespace AppCore.Hosting.Microsoft.Extensions
{
    public class BackgroundServiceHostAdapterTests
    {
        [Fact]
        public async Task StartStartsHost()
        {
            var host = Substitute.For<IBackgroundServiceHost>();
            var adapter = new BackgroundServiceHostAdapter(host);
            
            await adapter.StartAsync(CancellationToken.None);

            await host.Received(1)
                      .StartAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task StopStopsHost()
        {
            var host = Substitute.For<IBackgroundServiceHost>();
            var adapter = new BackgroundServiceHostAdapter(host);
            
            await adapter.StopAsync(CancellationToken.None);

            await host.Received(1)
                      .StopAsync(Arg.Any<CancellationToken>());
        }
    }
}

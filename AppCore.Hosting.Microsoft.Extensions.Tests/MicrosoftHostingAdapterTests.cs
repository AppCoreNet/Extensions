// Licensed under the MIT License.
// Copyright (c) 2018-2020 the AppCore .NET project.

using System;
using System.Threading;
using System.Threading.Tasks;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Xunit;

namespace AppCore.Hosting.Microsoft.Extensions
{
    public class MicrosoftHostingAdapterTests
    {
        [Fact]
        public async Task StartsBackgroundServices()
        {
            var startupTaskExecutor = Substitute.For<IStartupTaskExecutor>();
            var backgroundServiceHost = Substitute.For<IBackgroundServiceHost>();

            var adapter = new MicrosoftHostingAdapter(startupTaskExecutor, backgroundServiceHost);
            await adapter.StartAsync(CancellationToken.None);

            await backgroundServiceHost.Received(1)
                                       .StartAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task ExecutesStartupTasks()
        {
            var startupTaskExecutor = Substitute.For<IStartupTaskExecutor>();
            var backgroundServiceHost = Substitute.For<IBackgroundServiceHost>();

            var adapter = new MicrosoftHostingAdapter(startupTaskExecutor, backgroundServiceHost);
            await adapter.StartAsync(CancellationToken.None);

            await startupTaskExecutor.Received(1)
                                     .ExecuteAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task DoesNotStartupBackgroundServicesWhenStartupTaskThrows()
        {
            var startupTaskExecutor = Substitute.For<IStartupTaskExecutor>();
            startupTaskExecutor.ExecuteAsync(Arg.Any<CancellationToken>())
                               .Throws(new InvalidOperationException());

            var backgroundServiceHost = Substitute.For<IBackgroundServiceHost>();

            var adapter = new MicrosoftHostingAdapter(startupTaskExecutor, backgroundServiceHost);
            try
            {
                await adapter.StartAsync(CancellationToken.None);
            }
            catch
            {
            }

            await backgroundServiceHost.DidNotReceive()
                                       .StartAsync(Arg.Any<CancellationToken>());
        }
        
        [Fact]
        public async Task StopsBackgroundServices()
        {
            var startupTaskExecutor = Substitute.For<IStartupTaskExecutor>();
            var backgroundServiceHost = Substitute.For<IBackgroundServiceHost>();

            var adapter = new MicrosoftHostingAdapter(startupTaskExecutor, backgroundServiceHost);
            await adapter.StopAsync(CancellationToken.None);

            await backgroundServiceHost.Received(1)
                      .StopAsync(Arg.Any<CancellationToken>());
        }
    }
}

// Licensed under the MIT License.
// Copyright (c) 2018-2020 the AppCore .NET project.

using System.Threading;
using System.Threading.Tasks;
using AppCore.Diagnostics;
using Microsoft.Extensions.Hosting;

namespace AppCore.Hosting.Microsoft.Extensions
{
    /// <summary>
    /// Provides the adapter for the Microsoft.Extensions.Hosting based application host.
    /// </summary>
    public class MicrosoftHostingAdapter : IHostedService
    {
        private readonly IBackgroundServiceHost _serviceHost;
        private IStartupTaskExecutor _startupTaskExecutor;

        /// <summary>
        /// Initializes a new instance of the <see cref="MicrosoftHostingAdapter"/> class.
        /// </summary>
        /// <param name="startupTaskExecutor">The startup task executor.</param>
        /// <param name="serviceHost">The background service host.</param>
        public MicrosoftHostingAdapter(IStartupTaskExecutor startupTaskExecutor, IBackgroundServiceHost serviceHost)
        {
            Ensure.Arg.NotNull(startupTaskExecutor, nameof(startupTaskExecutor));
            Ensure.Arg.NotNull(serviceHost, nameof(serviceHost));

            _startupTaskExecutor = startupTaskExecutor;
            _serviceHost = serviceHost;
        }

        /// <inheritdoc />
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _startupTaskExecutor.ExecuteAsync(cancellationToken)
                                      .ConfigureAwait(false);

            _startupTaskExecutor = null;

            await _serviceHost.StartAsync(cancellationToken)
                              .ConfigureAwait(false);
        }

        /// <inheritdoc />
        public Task StopAsync(CancellationToken cancellationToken)
        {
            return _serviceHost.StopAsync(cancellationToken);
        }
    }
}
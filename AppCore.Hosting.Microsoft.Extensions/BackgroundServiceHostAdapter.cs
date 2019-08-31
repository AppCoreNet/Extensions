// Licensed under the MIT License.
// Copyright (c) 2018 the AppCore .NET project.

using System.Threading;
using System.Threading.Tasks;
using AppCore.Diagnostics;
using Microsoft.Extensions.Hosting;

namespace AppCore.Hosting.Microsoft.Extensions
{
    public class BackgroundServiceHostAdapter : IHostedService
    {
        private readonly IBackgroundServiceHost _serviceHost;

        public BackgroundServiceHostAdapter(IBackgroundServiceHost serviceHost)
        {
            Ensure.Arg.NotNull(serviceHost, nameof(serviceHost));
            _serviceHost = serviceHost;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return _serviceHost.StartAsync(cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return _serviceHost.StopAsync(cancellationToken);
        }
    }
}
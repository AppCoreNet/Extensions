// Licensed under the MIT License.
// Copyright (c) 2018-2020 the AppCore .NET project.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AppCore.Diagnostics;
using AppCore.Logging;

namespace AppCore.Hosting
{
    /// <summary>
    /// Provides the default implementation of the <see cref="IBackgroundServiceHost"/>.
    /// </summary>
    public class BackgroundServiceHost : IBackgroundServiceHost
    {
        private readonly List<IBackgroundService> _services;
        private readonly ILogger<BackgroundServiceHost> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="BackgroundServiceHost"/> class.
        /// </summary>
        /// <param name="services">The background services which are hosted.</param>
        /// <param name="logger">The logger.</param>
        public BackgroundServiceHost(IEnumerable<IBackgroundService> services, ILogger<BackgroundServiceHost> logger)
        {
            Ensure.Arg.NotNull(services, nameof(services));
            Ensure.Arg.NotNull(logger, nameof(logger));

            _services = services.ToList();
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var services = new List<IBackgroundService>(_services.Count);

            var cancellationTokenSource = new CancellationTokenSource();
            using (cancellationToken.Register(() => cancellationTokenSource.Cancel(false)))
            {
                try
                {
                    IEnumerable<Task> tasks =
                        _services.Select(s => StartService(s, services, cancellationTokenSource));

                    await Task.WhenAll(tasks)
                              .ConfigureAwait(false);
                }
                catch (Exception)
                {
                    // stop already started services when some service failed to start
                    try
                    {
                        var stopCancellationTokenSource = new CancellationTokenSource();
                        using (cancellationToken.Register(() => stopCancellationTokenSource.Cancel(false)))
                        {
                            IEnumerable<Task> tasks = services.Select(s => StopService(s, stopCancellationTokenSource));
                            await Task.WhenAll(tasks)
                                      .ConfigureAwait(false);
                        }
                    }
                    catch
                    {
                        // ignored
                    }

                    throw;
                }
            }
        }

        private async Task StartService(
            IBackgroundService service,
            List<IBackgroundService> startedServices,
            CancellationTokenSource cancellationTokenSource)
        {
            _logger.ServiceStarting(service);

            var stopwatch = Stopwatch.StartNew();
            try
            {
                await service.StartAsync(cancellationTokenSource.Token);
                startedServices.Add(service);
                _logger.ServiceStarted(service, stopwatch.Elapsed);
            }
            catch (Exception error)
            {
                _logger.ServiceStartFailed(service, stopwatch.Elapsed, error);
                cancellationTokenSource.Cancel(false);
                throw;
            }
        }

        private async Task StopService(
            IBackgroundService service,
            CancellationTokenSource cancellationTokenSource)
        {
            _logger.ServiceStopping(service);

            var stopwatch = Stopwatch.StartNew();
            try
            {
                await service.StopAsync(cancellationTokenSource.Token);
                _logger.ServiceStopped(service, stopwatch.Elapsed);
            }
            catch (Exception error)
            {
                _logger.ServiceStopFailed(service, stopwatch.Elapsed, error);
                cancellationTokenSource.Cancel(false);
                throw;
            }
        }

        /// <inheritdoc />
        public async Task StopAsync(CancellationToken cancellationToken)
        {
            var cancellationTokenSource = new CancellationTokenSource();
            using (cancellationToken.Register(() => cancellationTokenSource.Cancel(false)))
            {
                IEnumerable<Task> tasks = _services.Select(s => StopService(s, cancellationTokenSource));
                await Task.WhenAll(tasks)
                          .ConfigureAwait(false);
            }
        }
    }
}

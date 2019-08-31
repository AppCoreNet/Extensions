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
    public class BackgroundServiceHost
    {
        private readonly List<IBackgroundService> _services;
        private readonly ILogger<BackgroundServiceHost> _logger;

        public BackgroundServiceHost(IEnumerable<IBackgroundService> services, ILogger<BackgroundServiceHost> logger)
        {
            Ensure.Arg.NotNull(services, nameof(services));
            Ensure.Arg.NotNull(logger, nameof(logger));

            _services = services.ToList();
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var startedServices = new List<IBackgroundService>(_services.Count);

            var cancellationTokenSource = new CancellationTokenSource();
            using (cancellationToken.Register(() => cancellationTokenSource.Cancel(false)))
            {
                try
                {
                    await Task.WhenAll(_services.Select(s => StartService(s, startedServices, cancellationTokenSource)));
                }
                catch (Exception)
                {
                    // stop already started services when some service failed to start
                    try
                    {
                        var stopCancellationTokenSource = new CancellationTokenSource();
                        using (cancellationToken.Register(() => stopCancellationTokenSource.Cancel(false)))
                        {
                            await Task.WhenAll(startedServices.Select(s => StopService(s, stopCancellationTokenSource)));
                        }
                    }
                    catch
                    {
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

            Stopwatch stopwatch = Stopwatch.StartNew();
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

            Stopwatch stopwatch = Stopwatch.StartNew();
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

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            var cancellationTokenSource = new CancellationTokenSource();
            using (cancellationToken.Register(() => cancellationTokenSource.Cancel(false)))
            {
                IEnumerable<Task> tasks = _services.Select(s => StopService(s, cancellationTokenSource));
                await Task.WhenAll(tasks);
            }
        }
    }
}

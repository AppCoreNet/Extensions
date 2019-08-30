using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AppCore.Diagnostics;

namespace AppCore.Hosting
{
    public class BackgroundServiceHost
    {
        private readonly List<IBackgroundService> _services;

        public BackgroundServiceHost(IEnumerable<IBackgroundService> services)
        {
            Ensure.Arg.NotNull(services, nameof(services));
            _services = services.ToList();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var startedServices = new List<IBackgroundService>(_services.Count);

            using (var startCancellationTokenSource = new CancellationTokenSource())
            using (cancellationToken.Register(() => startCancellationTokenSource.Cancel(false)))
            {
                cancellationToken = startCancellationTokenSource.Token;

                IEnumerable<Task> tasks =
                    _services.Select(
                                 s => new Func<Task>(
                                     async () =>
                                     {
                                         cancellationToken.ThrowIfCancellationRequested();
                                         try
                                         {
                                             await s.StartAsync(cancellationToken);
                                             startedServices.Add(s);
                                             Console.WriteLine($"Background service {s.GetType()} running.");
                                         }
                                         catch (Exception error)
                                         {
                                             startCancellationTokenSource.Cancel(false);
                                             Console.WriteLine($"Background service {s.GetType()} failed.");
                                             throw;
                                         }
                                     }))
                             .Select(s => s());

                try
                {
                    await Task.WhenAll(tasks);
                }
                catch (Exception)
                {
                    try
                    {
                        await StopInternal(startedServices, CancellationToken.None);
                    }
                    catch
                    {
                    }

                    throw;
                }
            }
        }

        private async Task StopInternal(IEnumerable<IBackgroundService> services, CancellationToken cancellationToken)
        {
            IEnumerable<Task> tasks =
                services.Select(
                             s => new Func<Task>(
                                 async () =>
                                 {
                                     cancellationToken.ThrowIfCancellationRequested();
                                     try
                                     {
                                         await s.StopAsync(cancellationToken);
                                         Console.WriteLine(
                                             $"Background service {s.GetType()} stopped.");
                                     }
                                     catch (Exception error)
                                     {
                                         Console.WriteLine($"Background service {s.GetType()} failed.");
                                         throw;
                                     }
                                 }))
                         .Select(s => s());

            await Task.WhenAll(tasks);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await StopInternal(_services, cancellationToken);
        }
    }
}

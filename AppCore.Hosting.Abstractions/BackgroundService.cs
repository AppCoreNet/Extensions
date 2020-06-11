// Licensed under the MIT License.
// Copyright (c) 2018,2019 the AppCore .NET project.

using System;
using System.Threading;
using System.Threading.Tasks;

namespace AppCore.Hosting
{
    /// <summary>
    /// Provides base implementation for the <see cref="IBackgroundService"/>.
    /// </summary>
    public abstract class BackgroundService : IBackgroundService, IDisposable
    {
        private CancellationTokenSource _shutdownCts = new CancellationTokenSource();
        private Task _backgroundTask;

        /// <inheritdoc />
        public Task StartAsync(CancellationToken cancellationToken)
        {
            if (_backgroundTask != null && !_backgroundTask.IsCompleted)
                throw new InvalidOperationException("Background task already running.");

            _backgroundTask = RunAsync(_shutdownCts.Token);
            return Task.FromResult(true);
        }

        /// <inheritdoc />
        public async Task StopAsync(CancellationToken cancellationToken)
        {
            if (_backgroundTask == null)
                return;

            try
            {
                _shutdownCts.Cancel();
            }
            finally
            {
                var tcs = new TaskCompletionSource<bool>();
                using (cancellationToken.Register(() => tcs.SetCanceled()))
                    await Task.WhenAny(_backgroundTask, tcs.Task);

                _shutdownCts.Dispose();
                _shutdownCts = new CancellationTokenSource();
            }
        }

        /// <summary>
        /// Must be implemented to run the service.
        /// </summary>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous event operation.</returns>
        protected abstract Task RunAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Cleans up managed resources.
        /// </summary>
        public void Dispose()
        {
            _shutdownCts.Dispose();
        }
    }
}
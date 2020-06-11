// Licensed under the MIT License.
// Copyright (c) 2018, 2019 the AppCore .NET project.

using System.Threading;
using System.Threading.Tasks;

namespace AppCore.Hosting
{
    /// <summary>
    /// Represents the host to start and stop background services.
    /// </summary>
    public interface IBackgroundServiceHost
    {
        /// <summary>
        /// Starts all background services.
        /// </summary>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous event operation.</returns>
        Task StartAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Stops all background services.
        /// </summary>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous event operation.</returns>
        Task StopAsync(CancellationToken cancellationToken);
    }
}
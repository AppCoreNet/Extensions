// Licensed under the MIT License.
// Copyright (c) 2018,2019 the AppCore .NET project.

using System.Threading;
using System.Threading.Tasks;

namespace AppCore.Hosting
{
    /// <summary>
    /// Represents a service which runs in the background for the lifetime of the application.
    /// </summary>
    public interface IBackgroundService
    {
        /// <summary>
        /// Starts the background service.
        /// </summary>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous event operation.</returns>
        Task StartAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Stops the background service.
        /// </summary>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous event operation.</returns>
        Task StopAsync(CancellationToken cancellationToken);
    }
}
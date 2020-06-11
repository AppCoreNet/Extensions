// Licensed under the MIT License.
// Copyright (c) 2020 the AppCore .NET project.

using System.Threading;
using System.Threading.Tasks;

namespace AppCore.Hosting
{
    /// <summary>
    /// Represents the executor for application startup tasks.
    /// </summary>
    public interface IStartupTaskExecutor
    {
        /// <summary>
        /// Executes all registered startup tasks.
        /// </summary>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous event operation.</returns>
        Task ExecuteAsync(CancellationToken cancellationToken);
    }
}
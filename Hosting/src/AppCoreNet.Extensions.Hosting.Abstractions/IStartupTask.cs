// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System.Threading;
using System.Threading.Tasks;

namespace AppCoreNet.Extensions.Hosting;

/// <summary>
/// Represents a task that is executed during application startup.
/// </summary>
public interface IStartupTask
{
    /// <summary>
    /// Gets the priority of the startup task.
    /// </summary>
    int Order { get; }

    /// <summary>
    /// Executes the initialization code.
    /// </summary>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous event operation.</returns>
    Task ExecuteAsync(CancellationToken cancellationToken = default);
}
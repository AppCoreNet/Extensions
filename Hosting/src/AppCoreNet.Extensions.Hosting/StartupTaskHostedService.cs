// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AppCoreNet.Diagnostics;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AppCoreNet.Extensions.Hosting;

/// <summary>
/// Provides an implementation of <see cref="IHostedService"/> which runs startup tasks.
/// </summary>
public sealed class StartupTaskHostedService : IHostedService
{
    private readonly IEnumerable<IStartupTask> _startupTasks;
    private readonly ILogger<StartupTaskHostedService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="StartupTaskHostedService"/> class.
    /// </summary>
    /// <param name="startupTasks">The startup tasks to execute.</param>
    /// <param name="logger">The logger.</param>
    public StartupTaskHostedService(IEnumerable<IStartupTask> startupTasks, ILogger<StartupTaskHostedService> logger)
    {
        Ensure.Arg.NotNull(startupTasks);
        Ensure.Arg.NotNull(logger);

        _startupTasks = startupTasks;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var stopwatch = new Stopwatch();
        foreach (IStartupTask startupTask in _startupTasks.OrderBy(s => s.Order))
        {
            _logger.TaskExecuting(startupTask);

            stopwatch.Start();
            try
            {
                await startupTask.ExecuteAsync(cancellationToken)
                                 .ConfigureAwait(false);
            }
            catch (Exception error)
            {
                _logger.TaskFailed(startupTask, stopwatch.Elapsed, error);
                throw;
            }

            _logger.TaskExecuted(startupTask, stopwatch.Elapsed);
            stopwatch.Reset();
        }
    }

    /// <inheritdoc />
    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
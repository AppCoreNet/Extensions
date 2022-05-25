// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AppCore.Diagnostics;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AppCore.Extensions.Hosting
{
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
            Ensure.Arg.NotNull(startupTasks, nameof(startupTasks));
            Ensure.Arg.NotNull(logger, nameof(logger));

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
}
// Licensed under the MIT License.
// Copyright (c) 2020 the AppCore .NET project.

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
    /// Provides the default implementation of the <see cref="IStartupTaskExecutor"/>.
    /// </summary>
    public class StartupTaskExecutor : IStartupTaskExecutor
    {
        private readonly IEnumerable<IStartupTask> _startupTasks;
        private readonly ILogger<StartupTaskExecutor> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="StartupTaskExecutor"/> class.
        /// </summary>
        /// <param name="startupTasks">The startup tasks to execute.</param>
        /// <param name="logger">The logger.</param>
        public StartupTaskExecutor(IEnumerable<IStartupTask> startupTasks, ILogger<StartupTaskExecutor> logger)
        {
            Ensure.Arg.NotNull(startupTasks, nameof(startupTasks));
            Ensure.Arg.NotNull(logger, nameof(logger));

            _startupTasks = startupTasks;
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task ExecuteAsync(CancellationToken cancellationToken)
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
    }
}
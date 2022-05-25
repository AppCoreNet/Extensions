// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using System;
using Microsoft.Extensions.Logging;

// ReSharper disable once CheckNamespace
namespace AppCore.Extensions.Hosting
{
    internal static class LoggerExtensions
    {
        private static readonly Action<ILogger, string, Exception?> _taskExecuting =
            LoggerMessage.Define<string>(
                LogLevel.Trace,
                LogEventIds.TaskExecuting,
                "Executing startup task {startupTaskType}...");

        private static readonly Action<ILogger, string, long, Exception?> _taskExecuted =
            LoggerMessage.Define<string, long>(
                LogLevel.Debug,
                LogEventIds.TaskExecuted,
                "Startup task {startupTaskType} finished in {elapsedTime} ms.");

        private static readonly Action<ILogger, string, long, Exception?> _taskFailed =
            LoggerMessage.Define<string, long>(
                LogLevel.Error,
                LogEventIds.TaskFailed,
                "Failed to execute {startupTaskType} after {elapsedTime} ms.");

        public static void TaskExecuting(this ILogger logger, IStartupTask task)
        {
            _taskExecuting(
                logger,
                task.GetType().GetDisplayName(),
                null);
        }

        public static void TaskExecuted(this ILogger logger, IStartupTask task, TimeSpan elapsed)
        {
            _taskExecuted(
                logger,
                task.GetType().GetDisplayName(),
                (long) elapsed.TotalMilliseconds,
                null);
        }

        public static void TaskFailed(this ILogger logger, IStartupTask task, TimeSpan elapsed, Exception exception)
        {
            _taskFailed(
                logger,
                task.GetType().GetDisplayName(),
                (long) elapsed.TotalMilliseconds,
                exception);
        }
    }
}

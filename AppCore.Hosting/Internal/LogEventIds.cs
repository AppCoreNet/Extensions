// Licensed under the MIT License.
// Copyright (c) 2018-2020 the AppCore .NET project.

using AppCore.Logging;

namespace AppCore.Hosting
{
    internal class LogEventIds
    {
        // BackgroundServiceHost log events

        public static readonly LogEventId ServiceStarting = new LogEventId(0, nameof(ServiceStarting));

        public static readonly LogEventId ServiceStarted = new LogEventId(1, nameof(ServiceStarted));

        public static readonly LogEventId ServiceStartFailed = new LogEventId(2, nameof(ServiceStartFailed));

        public static readonly LogEventId ServiceStopping = new LogEventId(3, nameof(ServiceStopping));

        public static readonly LogEventId ServiceStopped = new LogEventId(4, nameof(ServiceStopped));

        public static readonly LogEventId ServiceStopFailed = new LogEventId(5, nameof(ServiceStopFailed));

        // StartupTaskExecutor log events

        public static readonly LogEventId TaskExecuting = new LogEventId(0, nameof(TaskExecuting));

        public static readonly LogEventId TaskExecuted = new LogEventId(1, nameof(TaskExecuted));

        public static readonly LogEventId TaskFailed = new LogEventId(2, nameof(TaskFailed));

    }
}

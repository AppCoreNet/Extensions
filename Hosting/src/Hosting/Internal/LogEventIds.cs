// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using Microsoft.Extensions.Logging;

namespace AppCore.Hosting
{
    internal class LogEventIds
    {
        // StartupTaskHostedService log events

        public static readonly EventId TaskExecuting = new EventId(0, nameof(TaskExecuting));

        public static readonly EventId TaskExecuted = new EventId(1, nameof(TaskExecuted));

        public static readonly EventId TaskFailed = new EventId(2, nameof(TaskFailed));
    }
}

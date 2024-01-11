// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using Microsoft.Extensions.Logging;

// ReSharper disable once CheckNamespace
namespace AppCore.Extensions.Hosting;

internal class LogEventIds
{
    // StartupTaskHostedService log events
    public static readonly EventId TaskExecuting = new EventId(0, nameof(TaskExecuting));

    public static readonly EventId TaskExecuted = new EventId(1, nameof(TaskExecuted));

    public static readonly EventId TaskFailed = new EventId(2, nameof(TaskFailed));
}
// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;
using System.Threading;
using System.Threading.Tasks;

namespace AppCoreNet.Extensions.Hosting.Plugins.TestPlugin;

internal class InternalStartupTask : IStartupTask
{
    public int Order { get; }

    public Task ExecuteAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
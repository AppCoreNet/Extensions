// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;
using System.Threading;
using System.Threading.Tasks;

namespace AppCore.Extensions.Hosting.Plugins.TestPlugin;

public class PublicStartupTask : IStartupTask
{
    public int Order { get; }

    public Task ExecuteAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
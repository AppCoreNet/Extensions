// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace AppCore.Extensions.Hosting.Plugins.TestPlugin2;

public class TestFacilityService : IHostedService
{
    public IHostApplicationLifetime Lifetime { get; }

    public TestFacilityService(IHostApplicationLifetime lifetime)
    {
        Lifetime = lifetime;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
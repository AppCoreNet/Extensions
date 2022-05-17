// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace AppCore.Hosting.Plugins.TestPlugin
{
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
}
// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using System.Threading;
using System.Threading.Tasks;

namespace AppCore.Hosting.Plugins.TestPlugin2
{
    public class TestFacilityService : IBackgroundService
    {
        public IApplicationLifetime Lifetime { get; }

        public TestFacilityService(IApplicationLifetime lifetime)
        {
            Lifetime = lifetime;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}
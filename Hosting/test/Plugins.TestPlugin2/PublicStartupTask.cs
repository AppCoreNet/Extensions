// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using System;
using System.Threading;
using System.Threading.Tasks;

namespace AppCore.Hosting.Plugins.TestPlugin2
{
    public class PublicStartupTask : IStartupTask
    {
        public int Order { get; }

        public Task ExecuteAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}

// Licensed under the MIT License.
// Copyright (c) 2018 the AppCore .NET project.

using System.Collections.Generic;
using Microsoft.Extensions.Hosting;

namespace AppCore.Hosting.Microsoft.Extensions
{
    public class BackgroundServiceHostAdapter : BackgroundServiceHost, IHostedService
    {
        public BackgroundServiceHostAdapter(IEnumerable<IBackgroundService> services)
            : base(services)
        {
        }
    }
}
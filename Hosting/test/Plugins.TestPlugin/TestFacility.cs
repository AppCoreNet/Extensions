// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using AppCore.DependencyInjection.Facilities;
using Microsoft.Extensions.DependencyInjection;

namespace AppCore.Hosting.Plugins.TestPlugin
{
    public class TestFacility : Facility
    {
        protected override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);

            services.AddTransient<TestFacilityService>();
        }
    }
}

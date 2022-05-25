// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using System;
using AppCore.Extensions.DependencyInjection.Activator;
using AppCore.Extensions.DependencyInjection.Facilities;
using Microsoft.Extensions.DependencyInjection;

namespace AppCore.Extensions.Hosting.Plugins.TestPlugin
{
    public class TestFacilityWithInjectedServices : Facility
    {
        public TestFacilityWithInjectedServices(IActivator activator)
        {
            if (activator == null)
                throw new ArgumentNullException(nameof(activator));
        }

        protected override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);

            services.AddTransient<TestFacilityService>();
        }
    }
}
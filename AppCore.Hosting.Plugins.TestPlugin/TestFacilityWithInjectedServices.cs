// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using System;
using AppCore.DependencyInjection;
using AppCore.DependencyInjection.Facilities;

namespace AppCore.Hosting.Plugins.TestPlugin
{
    public class TestFacilityWithInjectedServices : Facility
    {
        public TestFacilityWithInjectedServices(IBackgroundServiceHost host)
        {
            if (host == null)
                throw new ArgumentNullException(nameof(host));
        }

        protected override void Build(IComponentRegistry registry)
        {
            base.Build(registry);

            registry.Add(ComponentRegistration.Transient<TestFacilityService, TestFacilityService>());
        }
    }
}
// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using AppCore.DependencyInjection;
using AppCore.DependencyInjection.Facilities;

namespace AppCore.Hosting.Plugins.TestPlugin
{
    public class TestFacility : Facility
    {
        protected override void Build(IComponentRegistry registry)
        {
            base.Build(registry);

            registry.Add(ComponentRegistration.Transient<TestFacilityService, TestFacilityService>());
        }
    }
}

// Licensed under the MIT License.
// Copyright (c) 2018 the AppCore .NET project.

using AppCore.DependencyInjection.Facilities;

namespace AppCore.DependencyInjection
{
    class TestFacility : Facility
    {
        protected override void RegisterComponents(IComponentRegistry registry)
        {
            registry.Register<ITestFacilityService>()
                    .Add<TestFacilityService>();
        }
    }
}
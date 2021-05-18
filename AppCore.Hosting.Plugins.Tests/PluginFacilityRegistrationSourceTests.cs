// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using AppCore.DependencyInjection;
using AppCore.DependencyInjection.Activator;
using AppCore.DependencyInjection.Facilities;
using FluentAssertions;
using Xunit;

namespace AppCore.Hosting.Plugins
{
    public class PluginFacilityRegistrationSourceTests
    {
        [Fact]
        public void RegistersFacility()
        {
            Facility.Activator = new StartupContainer()
                .Resolve<IActivator>();

            var registry = new TestComponentRegistry();
            registry.AddPlugins(
                p =>
                {
                    p.Configure(o => o.PluginAssemblies.Add(PluginPaths.TestPlugin));
                });

            registry.AddFacilitiesFrom(s => s.Plugins());

            registry.Should()
                    .Contain(
                        r =>
                            r.ContractType.Name == "TestFacilityService"
                            && r.ImplementationType.Name == "TestFacilityService"
                    );
        }
    }
}
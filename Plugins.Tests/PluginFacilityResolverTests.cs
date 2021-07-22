// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using System.Collections.Generic;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace AppCore.Hosting.Plugins
{
    public class PluginFacilityResolverTests
    {
        private class ServiceCollection : List<ServiceDescriptor>, IServiceCollection
        {
        }

        [Fact]
        public void RegistersFacilities()
        {
            PluginFacility.PluginManager = null;

            var services = new ServiceCollection();
            services.AddPlugins(
                p =>
                {
                    p.Configure(o =>
                    {
                        o.Assemblies.Add(PluginPaths.TestPlugin);
                        o.Assemblies.Add(PluginPaths.TestPlugin2);
                    });
                });

            services.AddFacilitiesFrom(s => s.Plugins());

            services.Should()
                    .Contain(
                        r =>
                            r.ServiceType.FullName == "AppCore.Hosting.Plugins.TestPlugin.TestFacilityService"
                            && r.ImplementationType.FullName == "AppCore.Hosting.Plugins.TestPlugin.TestFacilityService"
                    );

            services.Should()
                    .Contain(
                        r =>
                            r.ServiceType.FullName == "AppCore.Hosting.Plugins.TestPlugin2.TestFacilityService"
                            && r.ImplementationType.FullName == "AppCore.Hosting.Plugins.TestPlugin2.TestFacilityService"
                    );
        }

        [Fact]
        public void RegistersFacilityWithServices()
        {
            PluginFacility.PluginManager = null;

            var services = new ServiceCollection();
            services.AddPlugins(
                p =>
                {
                    p.Configure(o =>
                    {
                        o.Assemblies.Add(PluginPaths.TestPlugin);
                    });
                });

            services.AddFacilitiesFrom(s => s.Plugins());

            services.Should()
                    .Contain(
                        r =>
                            r.ServiceType.FullName == "AppCore.Hosting.Plugins.TestPlugin.TestFacilityService"
                            && r.ImplementationType.FullName == "AppCore.Hosting.Plugins.TestPlugin.TestFacilityService"
                    )
                    .And.HaveCount(6);
        }
    }
}
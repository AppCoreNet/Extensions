// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using System.Collections.Generic;
using System.Linq;
using AppCore.DependencyInjection;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Xunit;

namespace AppCore.Hosting.Plugins
{
    public class PluginFacilityResolverTests
    {
        #nullable disable
        private class ServiceCollection : List<ServiceDescriptor>, IServiceCollection
        {
        }
        #nullable restore

        [Fact]
        public void RegistersFacilities()
        {
            PluginFacility.PluginManager = null;

            var services = new ServiceCollection();
            services.AddAppCore().AddPlugins(
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
            services.AddAppCore().AddPlugins(
                p =>
                {
                    p.Configure(o =>
                    {
                        o.Assemblies.Add(PluginPaths.TestPlugin);
                    });
                });

            services.AddFacilitiesFrom(s => s.Plugins());

            IEnumerable<ServiceDescriptor> facilityServices =
                services.Where(
                    r =>
                        r.ServiceType.FullName == "AppCore.Hosting.Plugins.TestPlugin.TestFacilityService"
                        && r.ImplementationType.FullName == "AppCore.Hosting.Plugins.TestPlugin.TestFacilityService");

            facilityServices.Should()
                            .HaveCount(2);
        }
    }
}
// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using System;
using System.Collections.Generic;
using AppCore.DependencyInjection;
using AppCore.DependencyInjection.Activator;
using AppCore.DependencyInjection.Facilities;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace AppCore.Hosting.Plugins
{
    public class PluginFacilityRegistrationSourceTests
    {
        [Fact]
        public void RegistersFacilities()
        {
            PluginManager.Instance = null;

            Facility.Activator = new StartupContainer()
                .Resolve<IActivator>();

            var registry = new TestComponentRegistry();
            registry.AddPlugins(
                p =>
                {
                    p.Configure(o =>
                    {
                        o.Assemblies.Add(PluginPaths.TestPlugin);
                        o.Assemblies.Add(PluginPaths.TestPlugin2);
                    });
                });

            registry.AddFacilitiesFrom(s => s.Plugins());

            registry.Should()
                    .Contain(
                        r =>
                            r.ContractType.FullName == "AppCore.Hosting.Plugins.TestPlugin.TestFacilityService"
                            && r.ImplementationType.FullName == "AppCore.Hosting.Plugins.TestPlugin.TestFacilityService"
                    );

            registry.Should()
                    .Contain(
                        r =>
                            r.ContractType.FullName == "AppCore.Hosting.Plugins.TestPlugin2.TestFacilityService"
                            && r.ImplementationType.FullName == "AppCore.Hosting.Plugins.TestPlugin2.TestFacilityService"
                    );
        }

        [Fact]
        public void RegistersFacilityWithServices()
        {
            PluginManager.Instance = null;

            var serviceHost = Substitute.For<IBackgroundServiceHost>();

            Facility.Activator = new StartupContainer(
                    new[] {new KeyValuePair<Type, object>(typeof(IBackgroundServiceHost), serviceHost)})
                .Resolve<IActivator>();

            var registry = new TestComponentRegistry();
            registry.AddPlugins(
                p =>
                {
                    p.Configure(o =>
                    {
                        o.Assemblies.Add(PluginPaths.TestPlugin);
                    });
                });

            registry.AddFacilitiesFrom(s => s.Plugins());

            registry.Should()
                    .Contain(
                        r =>
                            r.ContractType.FullName == "AppCore.Hosting.Plugins.TestPlugin.TestFacilityService"
                            && r.ImplementationType.FullName == "AppCore.Hosting.Plugins.TestPlugin.TestFacilityService"
                    )
                    .And.HaveCount(3);
        }
    }
}
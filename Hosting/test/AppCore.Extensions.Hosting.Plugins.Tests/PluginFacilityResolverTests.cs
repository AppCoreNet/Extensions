// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using System.Collections.Generic;
using System.Linq;
using AppCore.Extensions.DependencyInjection;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace AppCore.Extensions.Hosting.Plugins;

public class PluginFacilityResolverTests
{
#nullable disable
    private class ServiceCollection : List<ServiceDescriptor>, IServiceCollection
    {
    }
#nullable restore

    [Fact]
    public void RegistersFacilitiesWithExtensions()
    {
        var services = new ServiceCollection();
        services.AddAppCore()
                .AddPlugins(
                    o =>
                    {
                        o.Assemblies.Add(PluginPaths.TestPlugin);
                        o.Assemblies.Add(PluginPaths.TestPlugin2);
                    });

        services.AddFacilitiesFrom(
            s => s.Plugins()
                  .AddExtensionsFrom(r => r.Plugins()));

        services.Should()
                .Contain(
                    r =>
                        r.ServiceType.FullName == "AppCore.Extensions.Hosting.Plugins.TestPlugin.TestFacilityExtensionService"
                        && r.ImplementationType.FullName == "AppCore.Extensions.Hosting.Plugins.TestPlugin.TestFacilityExtensionService"
                );

        services.Should()
                .Contain(
                    r =>
                        r.ServiceType.FullName == "AppCore.Extensions.Hosting.Plugins.TestPlugin2.TestFacilityExtensionService"
                        && r.ImplementationType.FullName == "AppCore.Extensions.Hosting.Plugins.TestPlugin2.TestFacilityExtensionService"
                );
    }

    [Fact]
    public void RegistersFacilities()
    {
        var services = new ServiceCollection();
        services.AddAppCore()
                .AddPlugins(
                    o =>
                    {
                        o.Assemblies.Add(PluginPaths.TestPlugin);
                        o.Assemblies.Add(PluginPaths.TestPlugin2);
                    });

        services.AddFacilitiesFrom(s => s.Plugins());

        services.Should()
                .Contain(
                    r =>
                        r.ServiceType.FullName == "AppCore.Extensions.Hosting.Plugins.TestPlugin.TestFacilityService"
                        && r.ImplementationType.FullName == "AppCore.Extensions.Hosting.Plugins.TestPlugin.TestFacilityService"
                );

        services.Should()
                .Contain(
                    r =>
                        r.ServiceType.FullName == "AppCore.Extensions.Hosting.Plugins.TestPlugin2.TestFacilityService"
                        && r.ImplementationType.FullName == "AppCore.Extensions.Hosting.Plugins.TestPlugin2.TestFacilityService"
                );
    }

    [Fact]
    public void RegistersFacilityWithServices()
    {
        var services = new ServiceCollection();
        services.AddAppCore()
                .AddPlugins(
                    o =>
                    {
                        o.Assemblies.Add(PluginPaths.TestPlugin);
                    });

        services.AddFacilitiesFrom(s => s.Plugins());

        IEnumerable<ServiceDescriptor> facilityServices =
            services.Where(
                r =>
                    r.ServiceType.FullName == "AppCore.Extensions.Hosting.Plugins.TestPlugin.TestFacilityService"
                    && r.ImplementationType.FullName == "AppCore.Extensions.Hosting.Plugins.TestPlugin.TestFacilityService");

        facilityServices.Should()
                        .HaveCount(2);
    }
}
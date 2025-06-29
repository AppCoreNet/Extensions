// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using AppCoreNet.Extensions.DependencyInjection;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace AppCoreNet.Extensions.Hosting.Plugins;

[RequiresUnreferencedCode("This test requires types that may be trimmed by the linker.")]
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
        services.AddPlugins(
                    o =>
                    {
                        o.Assemblies.Add(PluginPaths.TestPlugin);
                        o.Assemblies.Add(PluginPaths.TestPlugin2);
                    });

        services.AddFacilitiesFrom(
            s => s
                 .Plugins()
                 .AddExtensionsFrom(r => r.Plugins()));

        services.Should()
                .Contain(
                    r =>
                        r.ServiceType.FullName
                        == "AppCoreNet.Extensions.Hosting.Plugins.TestPlugin.TestFacilityExtensionService"
                        && r.ImplementationType!.FullName
                        == "AppCoreNet.Extensions.Hosting.Plugins.TestPlugin.TestFacilityExtensionService");

        services.Should()
                .Contain(
                    r =>
                        r.ServiceType.FullName
                        == "AppCoreNet.Extensions.Hosting.Plugins.TestPlugin.TestFacilityContractExtensionService"
                        && r.ImplementationType!.FullName
                        == "AppCoreNet.Extensions.Hosting.Plugins.TestPlugin.TestFacilityContractExtensionService");

        services.Should()
                .Contain(
                    r =>
                        r.ServiceType.FullName == "AppCoreNet.Extensions.Hosting.Plugins.TestPlugin2.TestFacilityExtensionService"
                        && r.ImplementationType!.FullName == "AppCoreNet.Extensions.Hosting.Plugins.TestPlugin2.TestFacilityExtensionService");
    }

    [Fact]
    public void RegistersFacilities()
    {
        var services = new ServiceCollection();
        services.AddPlugins(
                    o =>
                    {
                        o.Assemblies.Add(PluginPaths.TestPlugin);
                        o.Assemblies.Add(PluginPaths.TestPlugin2);
                    });

        services.AddFacilitiesFrom(s => s.Plugins());

        services.Should()
                .Contain(
                    r =>
                        r.ServiceType.FullName == "AppCoreNet.Extensions.Hosting.Plugins.TestPlugin.TestFacilityService"
                        && r.ImplementationType!.FullName == "AppCoreNet.Extensions.Hosting.Plugins.TestPlugin.TestFacilityService");

        services.Should()
                .Contain(
                    r =>
                        r.ServiceType.FullName == "AppCoreNet.Extensions.Hosting.Plugins.TestPlugin2.TestFacilityService"
                        && r.ImplementationType!.FullName == "AppCoreNet.Extensions.Hosting.Plugins.TestPlugin2.TestFacilityService");
    }

    [Fact]
    public void RegistersFacilityWithServices()
    {
        var services = new ServiceCollection();
        services.AddPlugins(
                    o =>
                    {
                        o.Assemblies.Add(PluginPaths.TestPlugin);
                    });

        services.AddFacilitiesFrom(s => s.Plugins());

        IEnumerable<ServiceDescriptor> facilityServices =
            services.Where(
                r =>
                    r.ServiceType.FullName == "AppCoreNet.Extensions.Hosting.Plugins.TestPlugin.TestFacilityService"
                    && r.ImplementationType!.FullName == "AppCoreNet.Extensions.Hosting.Plugins.TestPlugin.TestFacilityService");

        facilityServices.Should()
                        .HaveCount(2);
    }
}
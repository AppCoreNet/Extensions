// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System.Collections.Generic;
using AppCoreNet.Extensions.DependencyInjection;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace AppCoreNet.Extensions.Hosting.Plugins;

public class PluginServiceDescriptorResolverTests
{
#nullable disable
    private class ServiceCollection : List<ServiceDescriptor>, IServiceCollection
    {
    }
#nullable restore

    [Fact]
    public void RegistersServices()
    {
        var services = new ServiceCollection();
        services.AddPlugins(
                    o =>
                    {
                        o.Assemblies.Add(PluginPaths.TestPlugin);
                        o.Assemblies.Add(PluginPaths.TestPlugin2);
                    });

        services.TryAddEnumerableFrom<IStartupTask>(s => s.Plugins());

        services.Should()
                .Contain(
                    r =>
                        r.ServiceType.FullName == "AppCoreNet.Extensions.Hosting.IStartupTask"
                        && r.ImplementationType.FullName == "AppCoreNet.Extensions.Hosting.Plugins.TestPlugin.PublicStartupTask");

        services.Should()
                .Contain(
                    r =>
                        r.ServiceType.FullName == "AppCoreNet.Extensions.Hosting.IStartupTask"
                        && r.ImplementationType.FullName == "AppCoreNet.Extensions.Hosting.Plugins.TestPlugin2.PublicStartupTask");
    }
}
// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using System;
using System.Collections.Generic;
using System.Linq;
using AppCore.Extensions.DependencyInjection.Activator;
using FluentAssertions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using NSubstitute;
using Xunit;

namespace AppCore.Extensions.Hosting.Plugins;

public class PluginManagerTests
{
    [Fact]
    public void ResolveAllPublicInstancesFromPlugins()
    {
        var pluginOptions = new PluginOptions
        {
            Assemblies = { PluginPaths.TestPlugin, PluginPaths.TestPlugin2 }
        };

        var manager = new PluginManager(new DefaultActivator(), Options.Create(pluginOptions));
        manager.LoadPlugins();

        List<IPluginService<IStartupTask>> instances =
            manager.GetServices<IStartupTask>()
                   .ToList();

        instances.Should()
                 .HaveCount(2);

        instances.Select(i => i.Instance.GetType().FullName)
                 .Should()
                 .BeEquivalentTo(
                     "AppCore.Extensions.Hosting.Plugins.TestPlugin.PublicStartupTask",
                     "AppCore.Extensions.Hosting.Plugins.TestPlugin2.PublicStartupTask");
    }

    [Fact]
    public void ResolveAllInstancesFromPlugins()
    {
        var pluginOptions = new PluginOptions
        {
            ResolvePrivateTypes = true,
            Assemblies = {PluginPaths.TestPlugin, PluginPaths.TestPlugin2}
        };

        var manager = new PluginManager(new DefaultActivator(), Options.Create(pluginOptions));
        manager.LoadPlugins();

        List<IPluginService<IStartupTask>> instances =
            manager.GetServices<IStartupTask>()
                   .ToList();

        instances.Should()
                 .HaveCount(4);

        instances.Select(i => i.Instance.GetType().FullName)
                 .Should()
                 .BeEquivalentTo(
                     "AppCore.Extensions.Hosting.Plugins.TestPlugin.PublicStartupTask",
                     "AppCore.Extensions.Hosting.Plugins.TestPlugin2.PublicStartupTask",
                     "AppCore.Extensions.Hosting.Plugins.TestPlugin.InternalStartupTask",
                     "AppCore.Extensions.Hosting.Plugins.TestPlugin2.InternalStartupTask"
                 );
    }

    [Fact]
    public void GetAllPlugins()
    {
        var options = new PluginOptions();
        options.Assemblies.Add(PluginPaths.TestPlugin);
        options.Assemblies.Add(PluginPaths.TestPlugin2);

        var manager = new PluginManager(new DefaultActivator(), Options.Create(options));
        manager.Plugins.Select(p => p.Info)
               .Should()
               .BeEquivalentTo(
                   new[]
                   {
                       new PluginInfo(
                           "AppCore.Extensions.Hosting.Plugins.TestPlugin",
                           "11.10.0",
                           "Plugin1 Description",
                           "Plugin1 Copyright"),
                       new PluginInfo(
                           "AppCore.Extensions.Hosting.Plugins.TestPlugin2",
                           "12.10.0",
                           "Plugin2 Description",
                           "Plugin2 Copyright")
                   });
    }

    [Fact]
    public void LoadPluginsIgnoresUnknownPaths()
    {
        var options = new PluginOptions();
        options.Assemblies.Add(PluginPaths.TestPlugin);
        options.Assemblies.Add(PluginPaths.TestPlugin2);
        options.Assemblies.Add("ThisPluginDoesNotExist.dll");

        var manager = new PluginManager(new DefaultActivator(), Options.Create(options));
        manager.LoadPlugins();

        manager.Plugins.Should()
               .HaveCount(2);
    }

    [Fact]
    public void ResolveAllInjectsServices()
    {
        var options = new PluginOptions();
        options.Assemblies.Add(PluginPaths.TestPlugin);
        options.Assemblies.Add(PluginPaths.TestPlugin2);

        var lifetime = Substitute.For<IHostApplicationLifetime>();

        var serviceProvider = Substitute.For<IServiceProvider>();
        serviceProvider.GetService(typeof(IHostApplicationLifetime))
                       .Returns(lifetime);

        var manager = new PluginManager(new ServiceProviderActivator(serviceProvider), Options.Create(options));
        List<IPluginService<IHostedService>> instances =
            manager.GetServices<IHostedService>()
                   .ToList();

        instances.Select(
                     i =>
                         i.Instance.GetType()
                          .GetProperty("Lifetime")!
                          .GetValue(i.Instance))
                 .Should()
                 .AllBeEquivalentTo(lifetime);
    }

    [Fact]
    public void LoadDoesNotLoadDisabled()
    {
        var options = new PluginOptions();
        options.Assemblies.Add(PluginPaths.TestPlugin);
        options.Assemblies.Add(PluginPaths.TestPlugin2);
        options.Disabled.Add("AppCore.Extensions.Hosting.Plugins.TestPlugin2");

        var manager = new PluginManager(new DefaultActivator(), Options.Create(options));
        manager.LoadPlugins();

        manager.Plugins.Should()
               .HaveCount(1);
    }
}
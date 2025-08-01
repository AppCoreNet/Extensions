// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using AppCoreNet.Extensions.DependencyInjection.Activator;
using FluentAssertions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using NSubstitute;
using Xunit;

namespace AppCoreNet.Extensions.Hosting.Plugins;

[RequiresUnreferencedCode("This test requires types that may be trimmed by the linker.")]
public class PluginManagerTests
{
    [Fact]
    public void ResolveAllPublicInstancesFromPlugins()
    {
        var pluginOptions = new PluginOptions
        {
            Assemblies = { PluginPaths.TestPlugin, PluginPaths.TestPlugin2 },
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
                     "AppCoreNet.Extensions.Hosting.Plugins.TestPlugin.PublicStartupTask",
                     "AppCoreNet.Extensions.Hosting.Plugins.TestPlugin2.PublicStartupTask");
    }

    [Fact]
    public void ResolveAllInstancesFromPlugins()
    {
        var pluginOptions = new PluginOptions
        {
            ResolvePrivateTypes = true,
            Assemblies = { PluginPaths.TestPlugin, PluginPaths.TestPlugin2 },
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
                     "AppCoreNet.Extensions.Hosting.Plugins.TestPlugin.PublicStartupTask",
                     "AppCoreNet.Extensions.Hosting.Plugins.TestPlugin2.PublicStartupTask",
                     "AppCoreNet.Extensions.Hosting.Plugins.TestPlugin.InternalStartupTask",
                     "AppCoreNet.Extensions.Hosting.Plugins.TestPlugin2.InternalStartupTask");
    }

    [Fact]
    public void GetAllPlugins()
    {
        string version = typeof(PluginManagerTests)
                         .Assembly
                         .GetCustomAttribute<AssemblyInformationalVersionAttribute>()!
                         .InformationalVersion;

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
                           "AppCoreNet.Extensions.Hosting.Plugins.TestPlugin",
                           version,
                           "Plugin1 Description",
                           "Plugin1 Copyright"),
                       new PluginInfo(
                           "AppCoreNet.Extensions.Hosting.Plugins.TestPlugin2",
                           version,
                           "Plugin2 Description",
                           "Plugin2 Copyright"),
                   });
    }

    [Fact]
    public void LoadPluginsIgnoresUnknownPaths()
    {
        var options = new PluginOptions();
        options.Assemblies.Add(PluginPaths.TestPlugin);
        options.Assemblies.Add(PluginPaths.TestPlugin2);
        options.Assemblies.Add("ThisPluginDoesNotExist.dll");
        options.Enabled["*"] = true;

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
    [Obsolete]
    public void LoadDoesNotLoadDisabled()
    {
        var options = new PluginOptions();
        options.Assemblies.Add(PluginPaths.TestPlugin);
        options.Assemblies.Add(PluginPaths.TestPlugin2);
        options.Enabled["*"] = true;
        options.Disabled.Add("AppCoreNet.Extensions.Hosting.Plugins.TestPlugin2");

        var manager = new PluginManager(new DefaultActivator(), Options.Create(options));
        manager.LoadPlugins();

        manager.Plugins.Should()
               .HaveCount(1);
    }

    [Fact]
    public void LoadsOnlyExplicitlyEnabled()
    {
        var options = new PluginOptions();
        options.Assemblies.Add(PluginPaths.TestPlugin);
        options.Assemblies.Add(PluginPaths.TestPlugin2);
        options.Enabled["*"] = false;
        options.Enabled["AppCoreNet.Extensions.Hosting.Plugins.TestPlugin2"] = true;

        var manager = new PluginManager(new DefaultActivator(), Options.Create(options));
        manager.LoadPlugins();

        manager.Plugins.Should()
               .HaveCount(1);
    }

    [Fact]
    public void DoesNotLoadExplicitlyDisabled()
    {
        var options = new PluginOptions();
        options.Assemblies.Add(PluginPaths.TestPlugin);
        options.Assemblies.Add(PluginPaths.TestPlugin2);
        options.Enabled["*"] = true;
        options.Enabled["AppCoreNet.Extensions.Hosting.Plugins.TestPlugin2"] = false;

        var manager = new PluginManager(new DefaultActivator(), Options.Create(options));
        manager.LoadPlugins();

        manager.Plugins.Should()
               .HaveCount(1);
    }

    [Fact]
    public void LoadsAllPluginsWithoutExplicitConfig()
    {
        var options = new PluginOptions();
        options.Assemblies.Add(PluginPaths.TestPlugin);
        options.Assemblies.Add(PluginPaths.TestPlugin2);

        var manager = new PluginManager(new DefaultActivator(), Options.Create(options));
        manager.LoadPlugins();

        manager.Plugins.Should()
               .HaveCount(2);
    }

    [Fact]
    public void LoadsAllPlugins()
    {
        var options = new PluginOptions();
        options.Assemblies.Add(PluginPaths.TestPlugin);
        options.Assemblies.Add(PluginPaths.TestPlugin2);
        options.Enabled["*"] = true;

        var manager = new PluginManager(new DefaultActivator(), Options.Create(options));
        manager.LoadPlugins();

        manager.Plugins.Should()
               .HaveCount(2);
    }

    [Fact]
    public void LoadsNoPlugins()
    {
        var options = new PluginOptions();
        options.Assemblies.Add(PluginPaths.TestPlugin);
        options.Assemblies.Add(PluginPaths.TestPlugin2);
        options.Enabled["*"] = false;

        var manager = new PluginManager(new DefaultActivator(), Options.Create(options));
        manager.LoadPlugins();

        manager.Plugins.Should()
               .HaveCount(0);
    }
}
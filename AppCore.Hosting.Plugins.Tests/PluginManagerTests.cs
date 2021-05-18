// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using System.Collections.Generic;
using System.Linq;
using AppCore.DependencyInjection.Activator;
using FluentAssertions;
using Xunit;

namespace AppCore.Hosting.Plugins
{
    public class PluginComponentRegistrationSourceTests
    {

    }

    public class PluginManagerTests
    {
        [Fact]
        public void ResolveAllPublicInstancesFromPlugins()
        {
            var pluginOptions = new PluginOptions
            {
                PluginAssemblies = { PluginPaths.TestPlugin }
            };

            var manager = new PluginManager(new DefaultActivator(), pluginOptions);
            manager.LoadPlugins();

            List<IPluginService<IStartupTask>> instances =
                manager.ResolveAll<IStartupTask>()
                       .ToList();

            instances.Should()
                     .HaveCount(1);
        }

        [Fact]
        public void ResolveAllInstancesFromPlugins()
        {
            var pluginOptions = new PluginOptions
            {
                ResolvePrivateTypes = true,
                PluginAssemblies = { PluginPaths.TestPlugin }
            };

            var manager = new PluginManager(new DefaultActivator(), pluginOptions);
            manager.LoadPlugins();

            List<IPluginService<IStartupTask>> instances =
                manager.ResolveAll<IStartupTask>()
                       .ToList();

            instances.Should()
                     .HaveCount(2);
        }

        [Fact]
        public void GetAllPlugins()
        {
            var options = new PluginOptions();
            options.PluginAssemblies.Add(PluginPaths.TestPlugin);

            var manager = new PluginManager(new DefaultActivator(), options);
            manager.Plugins.Select(p => p.Info).Should()
                   .BeEquivalentTo(
                       new PluginInfo(
                           "AppCore.Hosting.Plugins.TestPlugin",
                           "11.10.0",
                           "Plugin1 Description",
                           "Plugin1 Copyright"));
        }
    }
}

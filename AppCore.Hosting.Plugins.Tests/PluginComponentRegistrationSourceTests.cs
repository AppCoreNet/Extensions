// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using AppCore.DependencyInjection;
using AppCore.DependencyInjection.Activator;
using AppCore.DependencyInjection.Facilities;
using FluentAssertions;
using Xunit;

namespace AppCore.Hosting.Plugins
{
    public class PluginComponentRegistrationSourceTests
    {
        [Fact]
        public void RegistersServices()
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

            registry.TryAddEnumerableFrom(
                s =>
                    s.Plugins(
                        p =>
                            p.WithContract<IStartupTask>()));

            registry.Should()
                    .Contain(
                        r =>
                            r.ContractType.FullName == "AppCore.Hosting.IStartupTask"
                            && r.ImplementationType.FullName == "AppCore.Hosting.Plugins.TestPlugin.PublicStartupTask"
                    );

            registry.Should()
                    .Contain(
                        r =>
                            r.ContractType.FullName == "AppCore.Hosting.IStartupTask"
                            && r.ImplementationType.FullName == "AppCore.Hosting.Plugins.TestPlugin2.PublicStartupTask"
                    );
        }
    }
}
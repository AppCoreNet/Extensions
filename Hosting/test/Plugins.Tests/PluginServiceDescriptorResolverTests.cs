// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using System.Collections.Generic;
using AppCore.DependencyInjection;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace AppCore.Hosting.Plugins
{
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
            services.AddAppCore()
                    .AddPlugins(
                        o =>
                        {
                            o.Assemblies.Add(PluginPaths.TestPlugin);
                            o.Assemblies.Add(PluginPaths.TestPlugin2);
                        });

            services.TryAddEnumerableFrom<IStartupTask>(s => s.Plugins());

            services.Should()
                    .Contain(
                        r =>
                            r.ServiceType.FullName == "AppCore.Hosting.IStartupTask"
                            && r.ImplementationType.FullName == "AppCore.Hosting.Plugins.TestPlugin.PublicStartupTask"
                    );

            services.Should()
                    .Contain(
                        r =>
                            r.ServiceType.FullName == "AppCore.Hosting.IStartupTask"
                            && r.ImplementationType.FullName == "AppCore.Hosting.Plugins.TestPlugin2.PublicStartupTask"
                    );
        }
    }
}
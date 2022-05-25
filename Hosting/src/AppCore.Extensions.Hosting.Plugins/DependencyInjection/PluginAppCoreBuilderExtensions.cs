// Licensed under the MIT License.
// Copyright (c) 2018-2022 the AppCore .NET project.

using System;
using AppCore.Diagnostics;
using AppCore.Extensions.DependencyInjection.Activator;
using AppCore.Extensions.Hosting.Plugins;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

// ReSharper disable once CheckNamespace
namespace AppCore.Extensions.DependencyInjection
{
    /// <summary>
    /// Provides extension methods to register plugins.
    /// </summary>
    public static class PluginAppCoreBuilderExtensions
    {
        private static Func<IServiceProvider, PluginManager>? _pluginManagerFactory;
        private static PluginManager? _pluginManager;

        /// <summary>
        /// Adds plugins.
        /// </summary>
        /// <param name="builder">The <see cref="IAppCoreBuilder"/>.</param>
        /// <param name="configure">The configuration delegate.</param>
        /// <returns>The passed <see cref="IAppCoreBuilder"/> to allow chaining.</returns>
        public static IAppCoreBuilder AddPlugins(
            this IAppCoreBuilder builder,
            Action<PluginOptions>? configure = null)
        {
            Ensure.Arg.NotNull(builder, nameof(builder));

            IServiceCollection services = builder.Services;

            if (configure != null)
            {
                services.Configure(configure);

                // plugin manager was already created, re-create with possible options
                if (_pluginManagerFactory != null)
                {
                    PluginManager pluginManager = _pluginManager!;
                    _pluginManager = null;
                    _pluginManagerFactory = sp => _pluginManager ??= new PluginManager(
                        pluginManager!,
                        sp.GetRequiredService<IActivator>(),
                        sp.GetRequiredService<IOptions<PluginOptions>>());
                }
            }

            services.TryAddTransient<IActivator, ServiceProviderActivator>();
            services.TryAddTransient(typeof(IPluginService<>), typeof(PluginServiceWrapper<>));
            services.TryAddTransient(typeof(IPluginServiceCollection<>), typeof(PluginServiceCollectionWrapper<>));

            // resolve plugin manager via delegate, effectively it's a singleton
            services.TryAddTransient(GetOrCreatePluginManager);

            return builder;
        }

        private static IPluginManager GetOrCreatePluginManager(IServiceProvider serviceProvider)
        {
            // plugin manager is resolved for the first time, create it directly ...
            _pluginManagerFactory ??= sp => _pluginManager ??= new PluginManager(
                sp.GetRequiredService<IActivator>(),
                sp.GetRequiredService<IOptions<PluginOptions>>());

            return _pluginManagerFactory(serviceProvider);
        }
    }
}
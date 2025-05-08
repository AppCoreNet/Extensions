// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;
using AppCoreNet.Diagnostics;
using AppCoreNet.Extensions.DependencyInjection.Activator;
using AppCoreNet.Extensions.Hosting.Plugins;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

// ReSharper disable once CheckNamespace
namespace AppCoreNet.Extensions.DependencyInjection;

/// <summary>
/// Provides extension methods to register plugins.
/// </summary>
public static class PluginServiceCollectionExtensions
{
    private static readonly object _pluginManagerFactorySyncRoot = new();
    private static Func<IServiceProvider, PluginManager>? _pluginManagerFactory;
    private static PluginManager? _pluginManager;

    /// <summary>
    /// Adds plugins.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/>.</param>
    /// <param name="configure">The configuration delegate.</param>
    /// <returns>The passed <see cref="IServiceCollection"/> to allow chaining.</returns>
    public static IServiceCollection AddPlugins(
        this IServiceCollection services,
        Action<PluginOptions>? configure = null)
    {
        Ensure.Arg.NotNull(services);

        if (configure != null)
        {
            services.Configure(configure);

            // plugin manager was already created, re-create with possible options
            lock (_pluginManagerFactorySyncRoot)
            {
                if (_pluginManagerFactory != null)
                {
                    PluginManager pluginManager = _pluginManager!;
                    _pluginManager = null;
                    _pluginManagerFactory = sp => _pluginManager ??= new PluginManager(
                        pluginManager,
                        sp.GetRequiredService<IActivator>(),
                        sp.GetRequiredService<IOptions<PluginOptions>>());
                }
            }
        }

        services.TryAddTransient<IActivator, ServiceProviderActivator>();
        services.TryAddTransient(typeof(IPluginService<>), typeof(PluginServiceWrapper<>));
        services.TryAddTransient(typeof(IPluginServiceCollection<>), typeof(PluginServiceCollectionWrapper<>));

        // resolve plugin manager via delegate, effectively it's a singleton
        services.TryAddTransient(GetOrCreatePluginManager);

        return services;
    }

    private static IPluginManager GetOrCreatePluginManager(IServiceProvider serviceProvider)
    {
        lock (_pluginManagerFactorySyncRoot)
        {
            // plugin manager is resolved for the first time, create it directly ...
            _pluginManagerFactory ??= sp => _pluginManager ??= new PluginManager(
                sp.GetRequiredService<IActivator>(),
                sp.GetRequiredService<IOptions<PluginOptions>>());

            return _pluginManagerFactory(serviceProvider);
        }
    }
}
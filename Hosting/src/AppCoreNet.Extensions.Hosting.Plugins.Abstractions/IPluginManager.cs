// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;
using System.Collections.Generic;

namespace AppCoreNet.Extensions.Hosting.Plugins;

/// <summary>
/// Represents the plugin manager.
/// </summary>
public interface IPluginManager
{
    /// <summary>
    /// Gets the collection of loaded plugins.
    /// </summary>
    IReadOnlyCollection<IPlugin> Plugins { get; }

    /// <summary>
    /// Explicitly discovers and loads all plugins.
    /// </summary>
    void LoadPlugins();

    /// <summary>
    /// Gets the first instance of specified <paramref name="serviceType"/> exported from registered plugins.
    /// </summary>
    /// <param name="serviceType">The type of the service to resolve.</param>
    /// <returns>An enumerable of plugin instances.</returns>
    IPluginService<object>? GetService(Type serviceType);

    /// <summary>
    /// Gets all instances of specified <paramref name="serviceType"/> exported from registered plugins.
    /// </summary>
    /// <param name="serviceType">The type of the service to resolve.</param>
    /// <returns>An enumerable of plugin instances.</returns>
    IPluginServiceCollection<object> GetServices(Type serviceType);
}
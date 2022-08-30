// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using AppCore.Diagnostics;

namespace AppCore.Extensions.Hosting.Plugins;

/// <summary>
/// Provides extensions methods for the <seealso cref="IPluginManager"/>.
/// </summary>
public static class PluginManagerExtensions
{
    /// <summary>
    /// Gets all instances of specified type <typeparamref name="T"/> exported from registered plugins.
    /// </summary>
    /// <typeparam name="T">The type of the service to resolve.</typeparam>
    /// <returns>An enumerable of plugin instances.</returns>
    public static IPluginServiceCollection<T> GetServices<T>(this IPluginManager manager)
    {
        Ensure.Arg.NotNull(manager);
        return (IPluginServiceCollection<T>) manager.GetServices(typeof(T));
    }

    /// <summary>
    /// Gets the first instance of specified type <typeparamref name="T"/> exported from registered plugins.
    /// </summary>
    /// <typeparam name="T">The type of the service to resolve.</typeparam>
    /// <returns>An enumerable of plugin instances.</returns>
    public static IPluginService<T>? GetService<T>(this IPluginManager manager)
    {
        Ensure.Arg.NotNull(manager);
        return (IPluginService<T>?)manager.GetService(typeof(T));
    }
}
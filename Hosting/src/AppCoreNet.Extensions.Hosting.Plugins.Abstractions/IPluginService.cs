// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

namespace AppCore.Extensions.Hosting.Plugins;

/// <summary>
/// Represents a plugin service.
/// </summary>
/// <typeparam name="T">The type of the service.</typeparam>
public interface IPluginService<out T>
{
    /// <summary>
    /// Gets the service instance.
    /// </summary>
    T Instance { get; }

    /// <summary>
    /// Gets the plugin instance.
    /// </summary>
    IPlugin Plugin { get; }
}
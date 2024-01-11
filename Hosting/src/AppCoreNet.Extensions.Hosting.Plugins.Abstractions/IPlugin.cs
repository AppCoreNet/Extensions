// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;
using System.Reflection;

namespace AppCoreNet.Extensions.Hosting.Plugins;

/// <summary>
/// Represents a plugin.
/// </summary>
public interface IPlugin : IServiceProvider
{
    /// <summary>
    /// Gets the info about the plugin.
    /// </summary>
    PluginInfo Info { get; }

    /// <summary>
    /// Gets the plugin assembly.
    /// </summary>
    Assembly Assembly { get; }

    /// <summary>
    /// Loads a plugin assembly.
    /// </summary>
    /// <param name="fileName">The file name of the assembly.</param>
    /// <returns>The assembly (if found).</returns>
    Assembly LoadAssembly(string fileName);

    /// <summary>
    /// Enters the contextual reflection scope.
    /// </summary>
    /// <returns>The <see cref="IDisposable"/>.</returns>
    IDisposable EnterContextualReflection();
}
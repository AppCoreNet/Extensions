// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using System;
using System.Reflection;

namespace AppCore.Hosting.Plugins
{
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
        /// Enters the contextual reflection scope.
        /// </summary>
        /// <returns>The <see cref="IDisposable"/>.</returns>
        IDisposable EnterContextualReflection();
    }
}
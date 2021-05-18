// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using System;
using System.Collections.Generic;
using System.Reflection;

namespace AppCore.Hosting.Plugins
{
    /// <summary>
    /// Represents a plugin.
    /// </summary>
    public interface IPlugin
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
        /// Gets all instances of specified <paramref name="contractType"/> exported from the plugin.
        /// </summary>
        /// <param name="contractType">The type of the service to resolve.</param>
        /// <returns>An enumerable of plugin instances.</returns>
        IEnumerable<IPluginService<object>> ResolveAll(Type contractType);

        /// <summary>
        /// Enters the contextual reflection scope.
        /// </summary>
        /// <returns>The <see cref="IDisposable"/>.</returns>
        IDisposable EnterContextualReflection();
    }
}
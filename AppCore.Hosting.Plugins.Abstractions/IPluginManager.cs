// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using System;
using System.Collections.Generic;

namespace AppCore.Hosting.Plugins
{
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
        /// Gets all instances of specified <paramref name="contractType"/> exported from registered plugins.
        /// </summary>
        /// <param name="contractType">The type of the service to resolve.</param>
        /// <returns>An enumerable of plugin instances.</returns>
        IEnumerable<IPluginService<object>> ResolveAll(Type contractType);
    }
}
// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using System.Collections.Generic;
using System.Linq;
using AppCore.Diagnostics;

namespace AppCore.Hosting.Plugins
{
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
        public static IEnumerable<IPluginService<T>> ResolveAll<T>(this IPluginManager manager)
        {
            Ensure.Arg.NotNull(manager, nameof(manager));
            return manager.ResolveAll(typeof(T))
                          .Select(p => (IPluginService<T>)p);
        }
    }
}
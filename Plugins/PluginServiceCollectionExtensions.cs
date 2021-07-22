// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using System;
using AppCore.Diagnostics;
using AppCore.Hosting.Plugins;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Provides extension methods to register plugins.
    /// </summary>
    public static class PluginServiceCollectionExtensions
    {
        /// <summary>
        /// Adds plugins.
        /// </summary>
        /// <param name="registry">The <see cref="IServiceCollection"/>.</param>
        /// <param name="configure">The configuration delegate.</param>
        /// <returns>The passed <see cref="IServiceCollection"/> to allow chaining.</returns>
        public static IServiceCollection AddPlugins(
            this IServiceCollection registry,
            Action<PluginFacility> configure = null)
        {
            Ensure.Arg.NotNull(registry, nameof(registry));
            return registry.AddFacility(configure);
        }
    }
}
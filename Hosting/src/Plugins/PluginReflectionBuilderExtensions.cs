// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using System;
using AppCore.DependencyInjection;
using AppCore.DependencyInjection.Facilities;
using AppCore.Diagnostics;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Provides extension methods to register services and facilities from plugins.
    /// </summary>
    public static class PluginReflectionBuilderExtensions
    {
        /// <summary>
        /// Adds components by scanning plugin assemblies.
        /// </summary>
        /// <param name="sources">The <see cref="IServiceDescriptorReflectionBuilder"/>.</param>
        /// <param name="configure">The delegate to configure the <see cref="PluginServiceDescriptorResolver"/>.</param>
        /// <returns>The <see cref="IServiceDescriptorReflectionBuilder"/>.</returns>
        /// <exception cref="ArgumentNullException">Argument <paramref name="sources"/> or <paramref name="configure"/> is <c>null</c>. </exception>
        public static IServiceDescriptorReflectionBuilder Plugins(
            this IServiceDescriptorReflectionBuilder sources,
            Action<PluginServiceDescriptorResolver>? configure = null)
        {
            Ensure.Arg.NotNull(sources, nameof(sources));
            return sources.AddResolver(configure);
        }

        /// <summary>
        /// Adds facilities by scanning plugin assemblies.
        /// </summary>
        /// <param name="sources">The <see cref="IFacilityReflectionBuilder"/>.</param>
        /// <returns>The <see cref="IFacilityReflectionBuilder"/>.</returns>
        /// <exception cref="ArgumentNullException">Argument <paramref name="sources"/> is <c>null</c>. </exception>
        public static IFacilityReflectionBuilder Plugins(this IFacilityReflectionBuilder sources)
        {
            Ensure.Arg.NotNull(sources, nameof(sources));
            return sources.AddResolver<PluginFacilityResolver>();
        }
    }
}
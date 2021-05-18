// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using System;
using AppCore.DependencyInjection.Facilities;
using AppCore.Diagnostics;
using AppCore.Hosting;
using AppCore.Hosting.Plugins;

// ReSharper disable once CheckNamespace
namespace AppCore.DependencyInjection
{
    /// <summary>
    /// Provides extension methods to register plugins.
    /// </summary>
    public static class PluginRegistrationExtensions
    {
        /// <summary>
        /// Adds plugins.
        /// </summary>
        /// <param name="registry">The <see cref="IComponentRegistry"/>.</param>
        /// <param name="configure">The configuration delegate.</param>
        /// <returns>The <see cref="HostingFacility"/>.</returns>
        public static IComponentRegistry AddPlugins(
            this IComponentRegistry registry,
            Action<PluginHostingFacility> configure = null)
        {
            Ensure.Arg.NotNull(registry, nameof(registry));
            return registry.AddFacility(configure);
        }

        /// <summary>
        /// Adds components by scanning plugin assemblies.
        /// </summary>
        /// <param name="sources">The <see cref="IComponentRegistrationSources"/>.</param>
        /// <param name="configure">The delegate to configure the <see cref="PluginComponentRegistrationSource"/>.</param>
        /// <returns>The <see cref="IComponentRegistrationSources"/>.</returns>
        /// <exception cref="ArgumentNullException">Argument <paramref name="sources"/> or <paramref name="configure"/> is <c>null</c>. </exception>
        public static IComponentRegistrationSources Plugins(
            this IComponentRegistrationSources sources,
            Action<PluginComponentRegistrationSource> configure = null)
        {
            Ensure.Arg.NotNull(sources, nameof(sources));
            return sources.Add(configure);
        }

        /// <summary>
        /// Adds facilities by scanning plugin assemblies.
        /// </summary>
        /// <param name="sources">The <see cref="IFacilityRegistrationSources"/>.</param>
        /// <returns>The <see cref="IFacilityRegistrationSources"/>.</returns>
        /// <exception cref="ArgumentNullException">Argument <paramref name="sources"/> is <c>null</c>. </exception>
        public static IFacilityRegistrationSources Plugins(this IFacilityRegistrationSources sources)
        {
            Ensure.Arg.NotNull(sources, nameof(sources));
            return sources.Add<PluginFacilityRegistrationSource>();
        }
    }
}
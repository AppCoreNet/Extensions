// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using System;
using AppCore.DependencyInjection.Facilities;
using AppCore.Diagnostics;

namespace AppCore.DependencyInjection
{
    /// <summary>
    /// Provides extension methods to register facilities with the <see cref="IComponentRegistry"/> via assembly scanning.
    /// </summary>
    public static class AssemblyRegistrationExtensions
    {
        /// <summary>
        /// Adds components by scanning assemblies.
        /// </summary>
        /// <param name="sources">The <see cref="IComponentRegistrationSources"/>.</param>
        /// <param name="configure">The delegate to configure the <see cref="AssemblyComponentRegistrationSource"/>.</param>
        /// <returns>The <see cref="IComponentRegistrationSources"/>.</returns>
        /// <exception cref="ArgumentNullException">Argument <paramref name="sources"/> or <paramref name="configure"/> is <c>null</c>. </exception>
        public static IComponentRegistrationSources Assemblies(
            this IComponentRegistrationSources sources,
            Action<AssemblyComponentRegistrationSource> configure)
        {
            Ensure.Arg.NotNull(sources, nameof(sources));
            Ensure.Arg.NotNull(configure, nameof(configure));

            return sources.Add(configure);
        }

        /// <summary>
        /// Adds components by scanning assemblies.
        /// </summary>
        /// <param name="sources">The <see cref="IFacilityRegistrationSources"/>.</param>
        /// <param name="configure">The delegate to configure the <see cref="AssemblyFacilityRegistrationSource"/>.</param>
        /// <returns>The <see cref="IFacilityRegistrationSources"/>.</returns>
        /// <exception cref="ArgumentNullException">Argument <paramref name="sources"/> or <paramref name="configure"/> is <c>null</c>. </exception>
        public static IFacilityRegistrationSources Assemblies(
            this IFacilityRegistrationSources sources,
            Action<AssemblyFacilityRegistrationSource> configure)
        {
            Ensure.Arg.NotNull(sources, nameof(sources));
            Ensure.Arg.NotNull(configure, nameof(configure));

            return sources.Add(configure);
        }
    }
}

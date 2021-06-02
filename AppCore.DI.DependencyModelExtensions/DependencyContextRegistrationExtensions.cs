// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using System;
using AppCore.DependencyInjection.Facilities;
using AppCore.Diagnostics;

namespace AppCore.DependencyInjection
{
    /// <summary>
    /// Provides extension methods for registering components with the <see cref="IComponentRegistry"/> via assembly scanning.
    /// </summary>
    public static class DependencyContextRegistrationExtensions
    {
        /// <summary>
        /// Adds components by scanning all assemblies included in a <see cref="Microsoft.Extensions.DependencyModel.DependencyContext"/>.
        /// </summary>
        /// <param name="sources">The <see cref="IComponentRegistrationSources"/>.</param>
        /// <param name="configure">The delegate to configure the <see cref="DependencyContextComponentRegistrationSource"/>.</param>
        /// <returns>The <see cref="IComponentRegistrationSources"/>.</returns>
        /// <exception cref="ArgumentNullException">Argument <paramref name="sources"/> is <c>null</c>. </exception>
        public static IComponentRegistrationSources DependencyContext(
            this IComponentRegistrationSources sources,
            Action<DependencyContextComponentRegistrationSource> configure = null)
        {
            Ensure.Arg.NotNull(sources, nameof(sources));
            Ensure.Arg.NotNull(configure, nameof(configure));

            return sources.Add(configure);
        }

        /// <summary>
        /// Adds facilities by scanning all assemblies included in a <see cref="Microsoft.Extensions.DependencyModel.DependencyContext"/>.
        /// </summary>
        /// <param name="sources">The <see cref="IFacilityRegistrationSources"/>.</param>
        /// <param name="configure">The delegate to configure the <see cref="DependencyContextFacilityRegistrationSource"/>.</param>
        /// <returns>The <see cref="IComponentRegistrationSources"/>.</returns>
        /// <exception cref="ArgumentNullException">Argument <paramref name="sources"/> or <paramref name="configure"/> is <c>null</c>. </exception>
        public static IFacilityRegistrationSources DependencyContext(
            this IFacilityRegistrationSources sources,
            Action<DependencyContextFacilityRegistrationSource> configure)
        {
            Ensure.Arg.NotNull(sources, nameof(sources));
            Ensure.Arg.NotNull(configure, nameof(configure));

            return sources.Add(configure);
        }

    }
}

// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using System;
using AppCore.Diagnostics;

namespace AppCore.DependencyInjection
{
    /// <summary>
    /// Provides extension methods for registering components with the <see cref="IComponentRegistry"/> via assembly scanning.
    /// </summary>
    public static class DependencyContextRegistrationExtensions
    {
        /// <summary>
        /// Adds components by scanning assemblies all assemblies included in a <see cref="DependencyContext"/>.
        /// </summary>
        /// <param name="builder">The <see cref="IComponentRegistrationSources"/>.</param>
        /// <param name="configure">The delegate to configure the <see cref="DependencyContextComponentRegistrationSource"/>.</param>
        /// <returns>The <see cref="IComponentRegistrationSources"/>.</returns>
        /// <exception cref="ArgumentNullException">Argument <paramref name="builder"/> or <paramref name="configure"/> is <c>null</c>. </exception>
        public static IComponentRegistrationSources DependencyContext(
            this IComponentRegistrationSources builder,
            Action<DependencyContextComponentRegistrationSource> configure)
        {
            Ensure.Arg.NotNull(builder, nameof(builder));
            Ensure.Arg.NotNull(configure, nameof(configure));

            return builder.Add(configure);
        }
    }
}

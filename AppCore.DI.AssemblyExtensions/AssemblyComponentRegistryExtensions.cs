// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using System;
using AppCore.Diagnostics;

namespace AppCore.DependencyInjection
{
    /// <summary>
    /// Provides extension methods to register components with the <see cref="IComponentRegistry"/> via assembly scanning.
    /// </summary>
    public static class AssemblyComponentRegistryExtensions
    {
        /// <summary>
        /// Adds components to the <paramref name="registry"/> by scanning assemblies.
        /// </summary>
        /// <param name="registry">The <see cref="IComponentRegistry"/>.</param>
        /// <param name="configureBuilder">The method to configure the assembly registrations.</param>
        /// <returns>The <see cref="IComponentRegistry"/>.</returns>
        /// <exception cref="ArgumentNullException">Argument <paramref name="registry"/> or <paramref name="configureBuilder"/> is <c>null</c>. </exception>
        public static IComponentRegistry AddFromAssemblies(
            this IComponentRegistry registry,
            Action<AssemblyRegistrationBuilder> configureBuilder)
        {
            Ensure.Arg.NotNull(registry, nameof(registry));
            Ensure.Arg.NotNull(configureBuilder, nameof(configureBuilder));

            var builder = new AssemblyRegistrationBuilder();
            configureBuilder(builder);

            return registry.Add(builder.BuildRegistrations());
        }
    }
}

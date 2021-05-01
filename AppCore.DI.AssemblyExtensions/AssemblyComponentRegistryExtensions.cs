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
        /// Adds components by scanning assemblies.
        /// </summary>
        /// <param name="registry">The <see cref="IComponentRegistry"/>.</param>
        /// <param name="contractType">The type of the contract.</param>
        /// <param name="configureBuilder">The builder.</param>
        /// <returns>The <see cref="IComponentRegistry"/>.</returns>
        /// <exception cref="ArgumentNullException">Argument <paramref name="registry"/> or <paramref name="configureBuilder"/> is <c>null</c>. </exception>
        public static IComponentRegistry AddFromAssemblies(
            this IComponentRegistry registry,
            Type contractType,
            Action<AssemblyRegistrationBuilder> configureBuilder)
        {
            Ensure.Arg.NotNull(registry, nameof(registry));
            Ensure.Arg.NotNull(configureBuilder, nameof(configureBuilder));

            AssemblyRegistrationBuilder builder = AssemblyRegistrationBuilder.ForContract(contractType);
            configureBuilder(builder);

            return registry.Add(builder.BuildRegistrations());
        }

        /// <summary>
        /// Adds components by scanning assemblies.
        /// </summary>
        /// <typeparam name="TContract">The type of the contract.</typeparam>
        /// <param name="registry">The <see cref="IComponentRegistry"/>.</param>
        /// <param name="configureBuilder">The builder.</param>
        /// <returns>The <see cref="IComponentRegistry"/>.</returns>
        /// <exception cref="ArgumentNullException">Argument <paramref name="registry"/> or <paramref name="configureBuilder"/> is <c>null</c>. </exception>
        public static IComponentRegistry AddFromAssemblies<TContract>(
            this IComponentRegistry registry,
            Action<AssemblyRegistrationBuilder> configureBuilder)
            where TContract : class
        {
            return registry.AddFromAssemblies(typeof(TContract), configureBuilder);
        }
    }
}

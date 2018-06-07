// Licensed under the MIT License.
// Copyright (c) 2018 the AppCore .NET project.

using System;
using AppCore.Diagnostics;

namespace AppCore.DependencyInjection.Builder
{
    /// <summary>
    /// Provides extension methods for the <see cref="IComponentRegistrationBuilder{TRegistrationInfo}"/> type.
    /// </summary>
    public static class AssemblyComponentRegistrationBuilderExtensions
    {
        /// <summary>
        /// Adds a type filter when scanning assemblies.
        /// </summary>
        /// <param name="builder">The <see cref="IComponentRegistrationBuilder{TRegistrationInfo}"/>.</param>
        /// <param name="filter">The predicate used to filter types.</param>
        /// <returns>The <see cref="IComponentRegistrationBuilder{TRegistrationInfo}"/>.</returns>
        /// <exception cref="ArgumentNullException">
        ///     Argument <paramref name="builder"/> or <paramref name="filter"/> is <c>null</c>.
        /// </exception>
        public static IComponentRegistrationBuilder<AssemblyRegistrationInfo> WithFilter(
            this IComponentRegistrationBuilder<AssemblyRegistrationInfo> builder,
            Predicate<Type> filter)
        {
            Ensure.Arg.NotNull(builder, nameof(builder));
            Ensure.Arg.NotNull(filter, nameof(filter));

            builder.RegistrationInfo.Filters.Add(filter);
            return builder;
        }

        /// <summary>
        /// Adds a type filter when scanning assemblies.
        /// </summary>
        /// <param name="builder">The <see cref="IComponentRegistrationBuilder{TContract, TRegistrationInfo}"/>.</param>
        /// <param name="filter">The predicate used to filter types.</param>
        /// <returns>The <see cref="IComponentRegistrationBuilder{TContract, TRegistrationInfo}"/>.</returns>
        /// <exception cref="ArgumentNullException">
        ///     Argument <paramref name="builder"/> or <paramref name="filter"/> is <c>null</c>.
        /// </exception>
        public static IComponentRegistrationBuilder<TContract, AssemblyRegistrationInfo> WithFilter<TContract>(
            this IComponentRegistrationBuilder<TContract, AssemblyRegistrationInfo> builder,
            Predicate<Type> filter)
        {
            Ensure.Arg.NotNull(builder, nameof(builder));
            Ensure.Arg.NotNull(filter, nameof(filter));

            builder.RegistrationInfo.Filters.Add(filter);
            return builder;
        }
    }
}

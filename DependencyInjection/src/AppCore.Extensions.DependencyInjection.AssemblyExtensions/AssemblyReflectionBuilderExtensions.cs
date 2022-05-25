// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using System;
using AppCore.Diagnostics;
using AppCore.Extensions.DependencyInjection;
using AppCore.Extensions.DependencyInjection.Facilities;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Provides extension methods to resolve service descriptors and facilities by reflection.
    /// </summary>
    public static class AssemblyReflectionBuilderExtensions
    {
        /// <summary>
        /// Adds service descriptors by reflection.
        /// </summary>
        /// <param name="builder">The <see cref="IServiceDescriptorReflectionBuilder"/>.</param>
        /// <param name="configure">The delegate to configure the <see cref="AssemblyServiceDescriptorResolver"/>.</param>
        /// <returns>The <see cref="IServiceDescriptorReflectionBuilder"/>.</returns>
        /// <exception cref="ArgumentNullException">Argument <paramref name="builder"/> is <c>null</c>. </exception>
        public static IServiceDescriptorReflectionBuilder Assemblies(
            this IServiceDescriptorReflectionBuilder builder,
            Action<AssemblyServiceDescriptorResolver>? configure = null)
        {
            Ensure.Arg.NotNull(builder, nameof(builder));
            Ensure.Arg.NotNull(configure, nameof(configure));

            return builder.AddResolver(configure);
        }

        /// <summary>
        /// Adds service descriptors from the calling assembly by reflection.
        /// </summary>
        /// <param name="builder">The <see cref="IServiceDescriptorReflectionBuilder"/>.</param>
        /// <returns>The <see cref="IServiceDescriptorReflectionBuilder"/>.</returns>
        /// <exception cref="ArgumentNullException">Argument <paramref name="builder"/> is <c>null</c>. </exception>
        public static IServiceDescriptorReflectionBuilder Assembly(this IServiceDescriptorReflectionBuilder builder)
        {
            var assembly = System.Reflection.Assembly.GetCallingAssembly();
            return Assemblies(builder, c => c.Add(assembly));
        }

        /// <summary>
        /// Adds facilities by reflection.
        /// </summary>
        /// <param name="builder">The <see cref="IFacilityReflectionBuilder"/>.</param>
        /// <param name="configure">The delegate to configure the <see cref="AssemblyFacilityResolver"/>.</param>
        /// <returns>The <see cref="IFacilityReflectionBuilder"/>.</returns>
        /// <exception cref="ArgumentNullException">Argument <paramref name="builder"/> or <paramref name="configure"/> is <c>null</c>. </exception>
        public static IFacilityReflectionBuilder Assemblies(
            this IFacilityReflectionBuilder builder,
            Action<AssemblyFacilityResolver> configure)
        {
            Ensure.Arg.NotNull(builder, nameof(builder));
            Ensure.Arg.NotNull(configure, nameof(configure));

            return builder.AddResolver(configure);
        }

        /// <summary>
        /// Adds facilities from the calling assembly by reflection.
        /// </summary>
        /// <param name="builder">The <see cref="IFacilityReflectionBuilder"/>.</param>
        /// <param name="configure">The delegate to configure the <see cref="AssemblyFacilityResolver"/>.</param>
        /// <returns>The <see cref="IFacilityReflectionBuilder"/>.</returns>
        /// <exception cref="ArgumentNullException">Argument <paramref name="builder"/> or <paramref name="configure"/> is <c>null</c>. </exception>
        public static IFacilityReflectionBuilder Assembly(
            this IFacilityReflectionBuilder builder,
            Action<AssemblyFacilityResolver> configure)
        {
            var assembly = System.Reflection.Assembly.GetCallingAssembly();
            return Assemblies(builder, c => c.Add(assembly));
        }
    }
}

// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;
using AppCoreNet.Diagnostics;
using AppCoreNet.Extensions.DependencyInjection.Facilities;

namespace AppCoreNet.Extensions.DependencyInjection;

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
    /// <returns>The <see cref="IServiceDescriptorReflectionBuilder"/> to allow chaining.</returns>
    /// <exception cref="ArgumentNullException">Argument <paramref name="builder"/> is <c>null</c>. </exception>
    public static IServiceDescriptorReflectionBuilder Assemblies(
        this IServiceDescriptorReflectionBuilder builder,
        Action<AssemblyServiceDescriptorResolver>? configure = null)
    {
        Ensure.Arg.NotNull(builder);
        Ensure.Arg.NotNull(configure);

        return builder.AddResolver(configure);
    }

    /// <summary>
    /// Adds service descriptors from the calling assembly by reflection.
    /// </summary>
    /// <param name="builder">The <see cref="IServiceDescriptorReflectionBuilder"/>.</param>
    /// <returns>The <see cref="IServiceDescriptorReflectionBuilder"/> to allow chaining.</returns>
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
    /// <param name="configure">The delegate to configure the <see cref="AssemblyResolver"/>.</param>
    /// <returns>The <see cref="IFacilityReflectionBuilder"/> to allow chaining.</returns>
    /// <exception cref="ArgumentNullException">Argument <paramref name="builder"/> or <paramref name="configure"/> is <c>null</c>. </exception>
    public static IFacilityReflectionBuilder Assemblies(
        this IFacilityReflectionBuilder builder,
        Action<AssemblyResolver> configure)
    {
        Ensure.Arg.NotNull(builder);
        Ensure.Arg.NotNull(configure);

        return builder.AddResolver(configure);
    }

    /// <summary>
    /// Adds facilities from the calling assembly by reflection.
    /// </summary>
    /// <param name="builder">The <see cref="IFacilityReflectionBuilder"/>.</param>
    /// <param name="configure">The delegate to configure the <see cref="AssemblyResolver"/>.</param>
    /// <returns>The <see cref="IFacilityReflectionBuilder"/> to allow chaining.</returns>
    /// <exception cref="ArgumentNullException">Argument <paramref name="builder"/> or <paramref name="configure"/> is <c>null</c>. </exception>
    public static IFacilityReflectionBuilder Assembly(
        this IFacilityReflectionBuilder builder,
        Action<AssemblyResolver> configure)
    {
        var assembly = System.Reflection.Assembly.GetCallingAssembly();
        return Assemblies(builder, c => c.Add(assembly));
    }

    /// <summary>
    /// Adds facility extensions by reflection.
    /// </summary>
    /// <param name="builder">The <see cref="IFacilityExtensionReflectionBuilder"/>.</param>
    /// <param name="configure">The delegate to configure the <see cref="AssemblyResolver"/>.</param>
    /// <returns>The <see cref="IFacilityExtensionReflectionBuilder"/> to allow chaining.</returns>
    /// <exception cref="ArgumentNullException">Argument <paramref name="builder"/> or <paramref name="configure"/> is <c>null</c>. </exception>
    public static IFacilityExtensionReflectionBuilder Assemblies(
        this IFacilityExtensionReflectionBuilder builder,
        Action<AssemblyResolver> configure)
    {
        Ensure.Arg.NotNull(builder);
        Ensure.Arg.NotNull(configure);

        return builder.AddResolver(configure);
    }

    /// <summary>
    /// Adds facility extensions from the calling assembly by reflection.
    /// </summary>
    /// <param name="builder">The <see cref="IFacilityExtensionReflectionBuilder"/>.</param>
    /// <param name="configure">The delegate to configure the <see cref="AssemblyResolver"/>.</param>
    /// <returns>The <see cref="IFacilityExtensionReflectionBuilder"/> to allow chaining.</returns>
    /// <exception cref="ArgumentNullException">Argument <paramref name="builder"/> or <paramref name="configure"/> is <c>null</c>. </exception>
    public static IFacilityExtensionReflectionBuilder Assembly(
        this IFacilityExtensionReflectionBuilder builder,
        Action<AssemblyResolver> configure)
    {
        var assembly = System.Reflection.Assembly.GetCallingAssembly();
        return Assemblies(builder, c => c.Add(assembly));
    }
}
// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;
using AppCoreNet.Diagnostics;
using AppCoreNet.Extensions.DependencyInjection.Facilities;
using DependencyModel = Microsoft.Extensions.DependencyModel;

namespace AppCoreNet.Extensions.DependencyInjection;

/// <summary>
/// Provides extension methods to resolve service descriptors and facilities by reflection.
/// </summary>
public static class DependencyContextReflectionBuilderExtensions
{
    private static DependencyModel.DependencyContext ResolveDependencyContext(DependencyModel.DependencyContext? context)
    {
        context ??= DependencyModel.DependencyContext.Default;

        if (context == null)
        {
            throw new NotSupportedException(
                $"Argument {nameof(context)} is 'null' and no default DependencyContext was available.");
        }

        return context;
    }

    /// <summary>
    /// Adds service descriptors by reflection.
    /// </summary>
    /// <param name="builder">The <see cref="IServiceDescriptorReflectionBuilder"/>.</param>
    /// <param name="configure">The delegate to configure the <see cref="DependencyContextServiceDescriptorResolver"/>.</param>
    /// <returns>The <see cref="IServiceDescriptorReflectionBuilder"/> to allow chaining.</returns>
    /// <exception cref="ArgumentNullException">Argument <paramref name="builder"/> is <c>null</c>. </exception>
    public static IServiceDescriptorReflectionBuilder DependencyContext(
        this IServiceDescriptorReflectionBuilder builder,
        Action<DependencyContextServiceDescriptorResolver> configure)
    {
        Ensure.Arg.NotNull(builder);
        Ensure.Arg.NotNull(configure);

        return builder.AddResolver(configure);
    }

    /// <summary>
    /// Adds service descriptors by reflection.
    /// </summary>
    /// <param name="builder">The <see cref="IServiceDescriptorReflectionBuilder"/>.</param>
    /// <param name="context">The <see cref="DependencyModel.DependencyContext"/> to search for service descriptors.</param>
    /// <param name="configure">The delegate to configure the <see cref="DependencyContextServiceDescriptorResolver"/>.</param>
    /// <returns>The <see cref="IServiceDescriptorReflectionBuilder"/> to allow chaining.</returns>
    /// <exception cref="ArgumentNullException">Argument <paramref name="builder"/> is <c>null</c>. </exception>
    public static IServiceDescriptorReflectionBuilder DependencyContext(
        this IServiceDescriptorReflectionBuilder builder,
        DependencyModel.DependencyContext? context = null,
        Action<DependencyContextServiceDescriptorResolver>? configure = null)
    {
        context = ResolveDependencyContext(context);

        return DependencyContext(
            builder,
            c =>
            {
                c.Add(context);
                configure?.Invoke(c);
            });
    }

    /// <summary>
    /// Adds facilities by reflection.
    /// </summary>
    /// <param name="builder">The <see cref="IFacilityReflectionBuilder"/>.</param>
    /// <param name="configure">The delegate to configure the <see cref="DependencyContextResolver"/>.</param>
    /// <returns>The <see cref="IServiceDescriptorReflectionBuilder"/> to allow chaining.</returns>
    /// <exception cref="ArgumentNullException">Argument <paramref name="builder"/> or <paramref name="configure"/> is <c>null</c>. </exception>
    public static IFacilityReflectionBuilder DependencyContext(
        this IFacilityReflectionBuilder builder,
        Action<DependencyContextResolver> configure)
    {
        Ensure.Arg.NotNull(builder);
        Ensure.Arg.NotNull(configure);

        return builder.AddResolver(configure);
    }

    /// <summary>
    /// Adds facilities by reflection.
    /// </summary>
    /// <param name="builder">The <see cref="IFacilityReflectionBuilder"/>.</param>
    /// <param name="context">The <see cref="DependencyModel.DependencyContext"/> to search for facilities.</param>
    /// <param name="configure">The delegate to configure the <see cref="DependencyContextResolver"/>.</param>
    /// <returns>The <see cref="IServiceDescriptorReflectionBuilder"/> to allow chaining.</returns>
    /// <exception cref="ArgumentNullException">Argument <paramref name="builder"/> or <paramref name="configure"/> is <c>null</c>. </exception>
    public static IFacilityReflectionBuilder DependencyContext(
        this IFacilityReflectionBuilder builder,
        DependencyModel.DependencyContext? context = null,
        Action<DependencyContextResolver>? configure = null)
    {
        context = ResolveDependencyContext(context);

        return DependencyContext(
            builder,
            c =>
            {
                c.Add(context);
                configure?.Invoke(c);
            });
    }

    /// <summary>
    /// Adds facility extensions by reflection.
    /// </summary>
    /// <param name="builder">The <see cref="IFacilityExtensionReflectionBuilder"/>.</param>
    /// <param name="configure">The delegate to configure the <see cref="DependencyContextResolver"/>.</param>
    /// <returns>The <see cref="IFacilityExtensionReflectionBuilder"/> to allow chaining.</returns>
    /// <exception cref="ArgumentNullException">Argument <paramref name="builder"/> or <paramref name="configure"/> is <c>null</c>. </exception>
    public static IFacilityExtensionReflectionBuilder DependencyContext(
        this IFacilityExtensionReflectionBuilder builder,
        Action<DependencyContextResolver> configure)
    {
        Ensure.Arg.NotNull(builder);
        Ensure.Arg.NotNull(configure);

        return builder.AddResolver(configure);
    }

    /// <summary>
    /// Adds facility extensions by reflection.
    /// </summary>
    /// <param name="builder">The <see cref="IFacilityExtensionReflectionBuilder"/>.</param>
    /// <param name="context">The <see cref="DependencyModel.DependencyContext"/> to search for facilities.</param>
    /// <param name="configure">The delegate to configure the <see cref="DependencyContextResolver"/>.</param>
    /// <returns>The <see cref="IFacilityExtensionReflectionBuilder"/> to allow chaining.</returns>
    /// <exception cref="ArgumentNullException">Argument <paramref name="builder"/> or <paramref name="configure"/> is <c>null</c>.</exception>
    /// <exception cref="NotSupportedException">Argument <paramref name="context"/> is <c>null</c> and no default DependencyContext was available.</exception>
    public static IFacilityExtensionReflectionBuilder DependencyContext(
        this IFacilityExtensionReflectionBuilder builder,
        DependencyModel.DependencyContext? context = null,
        Action<DependencyContextResolver>? configure = null)
    {
        context = ResolveDependencyContext(context);

        return DependencyContext(
            builder,
            c =>
            {
                c.Add(context);
                configure?.Invoke(c);
            });
    }
}
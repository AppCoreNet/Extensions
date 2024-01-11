// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;
using AppCoreNet.Diagnostics;
using AppCoreNet.Extensions.DependencyInjection.Facilities;

// ReSharper disable once CheckNamespace
namespace AppCoreNet.Extensions.DependencyInjection;

/// <summary>
/// Provides extension methods to register services and facilities from plugins.
/// </summary>
public static class PluginReflectionBuilderExtensions
{
    /// <summary>
    /// Adds components by scanning plugin assemblies.
    /// </summary>
    /// <param name="sources">The <see cref="IServiceDescriptorReflectionBuilder"/>.</param>
    /// <param name="configure">The delegate to configure the <see cref="PluginServiceDescriptorResolver"/>.</param>
    /// <returns>The <see cref="IServiceDescriptorReflectionBuilder"/> to allow chaining.</returns>
    /// <exception cref="ArgumentNullException">Argument <paramref name="sources"/> or <paramref name="configure"/> is <c>null</c>. </exception>
    public static IServiceDescriptorReflectionBuilder Plugins(
        this IServiceDescriptorReflectionBuilder sources,
        Action<PluginServiceDescriptorResolver>? configure = null)
    {
        Ensure.Arg.NotNull(sources);
        return sources.AddResolver(configure);
    }

    /// <summary>
    /// Adds facilities by scanning plugin assemblies.
    /// </summary>
    /// <param name="sources">The <see cref="IFacilityReflectionBuilder"/>.</param>
    /// <returns>The <see cref="IFacilityReflectionBuilder"/> to allow chaining.</returns>
    /// <exception cref="ArgumentNullException">Argument <paramref name="sources"/> is <c>null</c>. </exception>
    public static IFacilityReflectionBuilder Plugins(this IFacilityReflectionBuilder sources)
    {
        Ensure.Arg.NotNull(sources);
        return sources.AddResolver<PluginFacilityResolver>();
    }

    /// <summary>
    /// Adds facility extensions by scanning plugin assemblies.
    /// </summary>
    /// <param name="sources">The <see cref="IFacilityExtensionReflectionBuilder"/>.</param>
    /// <returns>The <see cref="IFacilityReflectionBuilder"/> to allow chaining.</returns>
    /// <exception cref="ArgumentNullException">Argument <paramref name="sources"/> is <c>null</c>. </exception>
    public static IFacilityExtensionReflectionBuilder Plugins(this IFacilityExtensionReflectionBuilder sources)
    {
        Ensure.Arg.NotNull(sources);
        return sources.AddResolver<PluginFacilityResolver>();
    }
}
// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;
using System.ComponentModel;
using Microsoft.Extensions.DependencyInjection;

namespace AppCoreNet.Extensions.DependencyInjection;

/// <summary>
/// Represents a builder for resolving service descriptors by reflection.
/// </summary>
public interface IServiceDescriptorReflectionBuilder
{
    /// <summary>
    /// Specifies the default lifetime for resolved services.
    /// </summary>
    /// <param name="lifetime">The default lifetime.</param>
    /// <returns>The <see cref="IServiceDescriptorReflectionBuilder"/> to allow chaining.</returns>
    IServiceDescriptorReflectionBuilder WithDefaultLifetime(ServiceLifetime lifetime);

    /// <summary>
    /// Adds a service resolver to the builder.
    /// </summary>
    /// <param name="resolver">The resolver.</param>
    /// <returns>The <see cref="IServiceDescriptorReflectionBuilder"/> to allow chaining.</returns>
    /// <exception cref="ArgumentNullException">Argument <paramref name="resolver"/> is null.</exception>
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    IServiceDescriptorReflectionBuilder AddResolver(IServiceDescriptorResolver resolver);

    /// <summary>
    /// Adds a service resolver to the builder.
    /// </summary>
    /// <typeparam name="T">The type of the <see cref="IServiceDescriptorReflectionBuilder"/>.</typeparam>
    /// <param name="configure">The configuration delegate.</param>
    /// <returns>The <see cref="IServiceDescriptorReflectionBuilder"/> to allow chaining.</returns>
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public IServiceDescriptorReflectionBuilder AddResolver<T>(Action<T>? configure = null)
        where T : IServiceDescriptorResolver;
}
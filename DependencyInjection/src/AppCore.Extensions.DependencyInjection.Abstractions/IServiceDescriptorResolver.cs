// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Extensions.DependencyInjection;

namespace AppCore.Extensions.DependencyInjection;

/// <summary>
/// Represents a resolver for service descriptors.
/// </summary>
public interface IServiceDescriptorResolver
{
    /// <summary>
    /// Resolves the service descriptors.
    /// </summary>
    /// <param name="serviceType">The type of the service.</param>
    /// <param name="defaultLifetime">The default lifetime.</param>
    /// <returns>An <see cref="IEnumerable{T}"/> of <see cref="ServiceDescriptor"/>.</returns>
    /// <exception cref="ArgumentNullException">Argument <paramref name="serviceType"/> is null.</exception>
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    IEnumerable<ServiceDescriptor> Resolve(Type serviceType, ServiceLifetime defaultLifetime);
}
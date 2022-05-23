// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Extensions.DependencyInjection;

namespace AppCore.DependencyInjection
{
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
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        IEnumerable<ServiceDescriptor> Resolve(Type serviceType, ServiceLifetime defaultLifetime);
    }
}
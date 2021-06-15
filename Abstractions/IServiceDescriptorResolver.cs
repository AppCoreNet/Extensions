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
        /// Sets the service type of the services which will be resolved.
        /// </summary>
        /// <param name="serviceType">The type of the service.</param>
        void WithServiceType(Type serviceType);

        /// <summary>
        /// Specifies the default lifetime for services.
        /// </summary>
        /// <param name="lifetime">The default lifetime.</param>
        void WithDefaultLifetime(ServiceLifetime lifetime);

        /// <summary>
        /// Resolves the service descriptors.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{T}"/> of <see cref="ServiceDescriptor"/>.</returns>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        IEnumerable<ServiceDescriptor> Resolve();
    }
}
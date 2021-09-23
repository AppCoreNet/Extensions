// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using System;
using System.ComponentModel;
using Microsoft.Extensions.DependencyInjection;

namespace AppCore.DependencyInjection
{
    /// <summary>
    /// Represents a builder for resolving service descriptors by reflection.
    /// </summary>
    public interface IServiceDescriptorReflectionBuilder
    {
        /// <summary>
        /// Specifies the default lifetime for resolved services.
        /// </summary>
        /// <param name="lifetime">The default lifetime.</param>
        IServiceDescriptorReflectionBuilder WithDefaultLifetime(ServiceLifetime lifetime);

        /// <summary>
        /// Adds a service resolver to the builder.
        /// </summary>
        /// <param name="resolver">The resolver.</param>
        /// <returns>The <see cref="IServiceDescriptorReflectionBuilder"/> to allow chaining.</returns>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        IServiceDescriptorReflectionBuilder AddResolver(IServiceDescriptorResolver resolver);

        /// <summary>
        /// Adds a service resolver to the builder.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IServiceDescriptorReflectionBuilder"/>.</typeparam>
        /// <param name="configure">The configuration delegate.</param>
        /// <returns>The <see cref="IServiceDescriptorReflectionBuilder"/> to allow chaining.</returns>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public IServiceDescriptorReflectionBuilder AddResolver<T>(Action<T> configure = null)
            where T : IServiceDescriptorResolver, new();
    }
}
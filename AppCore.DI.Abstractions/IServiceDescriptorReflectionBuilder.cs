// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using System;
using System.ComponentModel;

namespace AppCore.DependencyInjection
{
    /// <summary>
    /// Represents a builder for resolving service descriptors by reflection.
    /// </summary>
    public interface IServiceDescriptorReflectionBuilder
    {
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
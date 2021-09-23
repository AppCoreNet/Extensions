// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using AppCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace AppCore.DependencyInjection
{
    /// <summary>
    /// Provides a default implementation of the <see cref="IServiceDescriptorReflectionBuilder"/> interface.
    /// </summary>
    internal class ServiceDescriptorReflectionBuilder : IServiceDescriptorReflectionBuilder
    {
        private readonly List<IServiceDescriptorResolver> _resolvers = new();
        private readonly Type _serviceType;
        private readonly ServiceLifetime _defaultLifetime;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceDescriptorReflectionBuilder"/> class.
        /// </summary>
        public ServiceDescriptorReflectionBuilder(
            Type serviceType = null,
            ServiceLifetime defaultLifetime = ServiceLifetime.Transient)
        {
            _serviceType = serviceType;
            _defaultLifetime = defaultLifetime;
        }

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public IServiceDescriptorReflectionBuilder AddResolver(IServiceDescriptorResolver resolver)
        {
            Ensure.Arg.NotNull(resolver, nameof(resolver));

            // configure the source
            resolver.WithDefaultLifetime(_defaultLifetime);
            if (_serviceType != null)
                resolver.WithServiceType(_serviceType);

            _resolvers.Add(resolver);
            return this;
        }

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public IServiceDescriptorReflectionBuilder AddResolver<T>(Action<T> configure = null)
            where T : IServiceDescriptorResolver, new()
        {
            var source = new T();
            configure?.Invoke(source);
            return AddResolver(source);
        }

        /// <summary>
        /// Builds the component registrations from all registered sources.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{T}"/> of <see cref="ServiceDescriptor"/>.</returns>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public IReadOnlyCollection<ServiceDescriptor> Resolve()
        {
            return _resolvers.SelectMany(s => s.Resolve())
                            .ToList()
                            .AsReadOnly();
        }
    }
}
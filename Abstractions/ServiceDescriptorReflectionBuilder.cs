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
    internal class ServiceDescriptorReflectionBuilder : IServiceDescriptorReflectionBuilder
    {
        private readonly List<IServiceDescriptorResolver> _resolvers = new();
        private readonly Type _serviceType;
        private ServiceLifetime _defaultLifetime = ServiceLifetime.Transient;

        public ServiceDescriptorReflectionBuilder(Type serviceType)
        {
            Ensure.Arg.NotNull(serviceType, nameof(serviceType));
            _serviceType = serviceType;
        }

        public IServiceDescriptorReflectionBuilder WithDefaultLifetime(ServiceLifetime lifetime)
        {
            _defaultLifetime = lifetime;
            return this;
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public IServiceDescriptorReflectionBuilder AddResolver(IServiceDescriptorResolver resolver)
        {
            Ensure.Arg.NotNull(resolver, nameof(resolver));
            _resolvers.Add(resolver);
            return this;
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public IServiceDescriptorReflectionBuilder AddResolver<T>(Action<T> configure = null)
            where T : IServiceDescriptorResolver, new()
        {
            var source = new T();
            configure?.Invoke(source);
            return AddResolver(source);
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public IReadOnlyCollection<ServiceDescriptor> Resolve()
        {
            return _resolvers.SelectMany(s => s.Resolve(_serviceType, _defaultLifetime))
                            .ToList()
                            .AsReadOnly();
        }
    }
}
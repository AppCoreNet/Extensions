// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using AppCore.Diagnostics;
using AppCore.Extensions.DependencyInjection.Activator;
using Microsoft.Extensions.DependencyInjection;

namespace AppCore.Extensions.DependencyInjection;

internal class ServiceDescriptorReflectionBuilder : IServiceDescriptorReflectionBuilder
{
    private readonly List<IServiceDescriptorResolver> _resolvers = new();
    private readonly IActivator _activator;
    private readonly Type _serviceType;
    private ServiceLifetime _defaultLifetime = ServiceLifetime.Transient;

    public ServiceDescriptorReflectionBuilder(IActivator activator, Type serviceType)
    {
        Ensure.Arg.NotNull(activator);
        Ensure.Arg.NotNull(serviceType);

        _activator = activator;
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
        Ensure.Arg.NotNull(resolver);
        _resolvers.Add(resolver);
        return this;
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public IServiceDescriptorReflectionBuilder AddResolver<T>(Action<T>? configure = null)
        where T : IServiceDescriptorResolver
    {
        var source = _activator.CreateInstance<T>();
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
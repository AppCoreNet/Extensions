// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using AppCoreNet.Diagnostics;
using AppCoreNet.Extensions.DependencyInjection.Activator;
using Microsoft.Extensions.DependencyInjection;

namespace AppCoreNet.Extensions.DependencyInjection;

internal sealed class ServiceDescriptorReflectionBuilder : IServiceDescriptorReflectionBuilder
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
    public IServiceDescriptorReflectionBuilder AddResolver<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
        T>(Action<T>? configure = null)
        where T : IServiceDescriptorResolver
    {
        var source = _activator.CreateInstance<T>()!;
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
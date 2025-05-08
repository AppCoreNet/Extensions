// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace AppCoreNet.Extensions.DependencyInjection;

/// <summary>
/// Internal service provider which resolves services from an <see cref="IServiceCollection"/>.
/// </summary>
internal sealed partial class ServiceCollectionServiceProvider : IServiceProvider, IServiceProviderIsService
{
    private readonly IServiceCollection _services;
    private readonly Dictionary<Type, object> _additionalServices = new();

    public ServiceCollectionServiceProvider(IServiceCollection services)
    {
        _services = services;
    }

    public void AddService(Type serviceType, object instance)
    {
        _additionalServices.Add(serviceType, instance);
    }

    public bool IsService(Type serviceType)
    {
        if (serviceType == typeof(IServiceProvider) || serviceType == typeof(IServiceProviderIsService))
            return true;

        if (_additionalServices.TryGetValue(serviceType, out object? _))
            return true;

        if (serviceType.IsGenericType && serviceType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
            return true;

        return _services.Any(
            sd => sd.ServiceType == serviceType
                  || (serviceType.IsGenericType
                      && sd.ServiceType == serviceType.GetGenericTypeDefinition()));
    }

    private IEnumerable<object> GetServices(Type serviceType)
    {
        object ServiceFactory(ServiceDescriptor serviceDescriptor)
        {
            object? instance = serviceDescriptor.ImplementationInstance;
            if (instance == null && serviceDescriptor.ImplementationFactory != null)
            {
                instance = serviceDescriptor.ImplementationFactory(this);
            }

            if (instance == null && serviceDescriptor.ImplementationType != null)
            {
                Type implementationType = serviceDescriptor.ImplementationType!;
                if (implementationType.IsGenericType)
                    implementationType = implementationType.MakeGenericType(serviceType.GenericTypeArguments);

                instance = ActivatorUtilities.CreateInstance(this, implementationType);
            }

            return instance!;
        }

        return _services.Where(
                            sd => sd.ServiceType == serviceType
                                  || (serviceType.IsGenericType
                                  && sd.ServiceType == serviceType.GetGenericTypeDefinition()))
                        .Select(ServiceFactory);
    }

    public object? GetService(Type serviceType)
    {
        if (serviceType == typeof(IServiceProvider) || serviceType == typeof(IServiceProviderIsService))
            return this;

        if (_additionalServices.TryGetValue(serviceType, out object? instance))
            return instance;

        if (serviceType.IsGenericType && serviceType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
        {
            serviceType = serviceType.GenericTypeArguments[0];
            var instances = (IList)System.Activator.CreateInstance(typeof(List<>).MakeGenericType(serviceType))!;
            foreach (object service in GetServices(serviceType))
            {
                instances.Add(service);
            }

            instance = instances;
        }
        else
        {
            instance = GetServices(serviceType).FirstOrDefault();
        }

        return instance;
    }
}
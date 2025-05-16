// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;

namespace AppCoreNet.Extensions.DependencyInjection;

/// <summary>
/// Internal service provider which resolves services from an <see cref="IServiceCollection"/>.
/// </summary>
internal sealed class ServiceCollectionServiceProvider : IServiceProvider, IServiceProviderIsService
{
    private readonly IServiceCollection _services;
    private readonly Dictionary<Type, object> _additionalServices = new();

    internal static bool VerifyAotCompatibility =>
#if NETFRAMEWORK || NETSTANDARD2_0
        false;
#else
        !RuntimeFeature.IsDynamicCodeSupported;
#endif

    public ServiceCollectionServiceProvider(IServiceCollection services)
    {
        _services = services;
    }

    public void AddService(Type serviceType, object instance)
    {
        _additionalServices.Add(serviceType, instance);
    }

    private static bool IsEnumerable(Type serviceType)
    {
        return serviceType.IsGenericType && serviceType.GetGenericTypeDefinition() == typeof(IEnumerable<>);
    }

    private static bool IsServiceType(Type serviceType, Type requestedServiceType)
    {
        return serviceType == requestedServiceType
               || (requestedServiceType.IsGenericType
                   && serviceType.IsGenericTypeDefinition
                   && serviceType == requestedServiceType.GetGenericTypeDefinition());
    }

    public bool IsService(Type serviceType)
    {
        if (serviceType == typeof(IServiceProvider) || serviceType == typeof(IServiceProviderIsService))
            return true;

        if (_additionalServices.TryGetValue(serviceType, out object? _))
            return true;

        if (IsEnumerable(serviceType))
            return true;

        return _services.Any(sd => IsServiceType(sd.ServiceType, serviceType));
    }

    private object[] GetServices(Type serviceType)
    {
        [UnconditionalSuppressMessage(
            "AotAnalysis",
            "IL3050:RequiresDynamicCode",
            Justification = "VerifyAotCompatibility ensures that dynamic code supported")]
        static Type MakeGenericType(Type type, params Type[] typeArguments)
        {
            if (VerifyAotCompatibility)
                throw new InvalidOperationException("Cannot build generic type when using AOT.");

            return type.MakeGenericType(typeArguments);
        }

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
                if (implementationType.IsGenericTypeDefinition)
                    implementationType = MakeGenericType(implementationType, serviceType.GenericTypeArguments);

                instance = ActivatorUtilities.CreateInstance(this, implementationType);
            }

            return instance!;
        }

        return _services.Where(sd => IsServiceType(sd.ServiceType, serviceType))
                        .Select(ServiceFactory)
                        .ToArray();
    }

    public object? GetService(Type serviceType)
    {
        if (serviceType == typeof(IServiceProvider) || serviceType == typeof(IServiceProviderIsService))
            return this;

        if (_additionalServices.TryGetValue(serviceType, out object? instance))
            return instance;

        if (IsEnumerable(serviceType))
        {
            serviceType = serviceType.GenericTypeArguments[0];
            object[] services = GetServices(serviceType);

            Array result = CreateArray(serviceType, services.Length);
            for (int i = 0; i < services.Length; i++)
            {
                result.SetValue(services[i], i);
            }

            instance = result;
        }
        else
        {
            object[] services = GetServices(serviceType);
            instance = services.Length > 0
                ? services[0]
                : null;
        }

        return instance;

        [UnconditionalSuppressMessage(
            "AotAnalysis",
            "IL3050:RequiresDynamicCode",
            Justification = "VerifyAotCompatibility ensures elementType is not a ValueType")]
        static Array CreateArray(Type elementType, int length)
        {
            if (VerifyAotCompatibility && elementType.IsValueType)
                throw new InvalidOperationException("Cannot build array of value service types.");

            return Array.CreateInstance(elementType, length);
        }
    }
}
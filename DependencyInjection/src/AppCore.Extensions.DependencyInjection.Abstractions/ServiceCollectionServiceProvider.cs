// Licensed under the MIT License.
// Copyright (c) 2018-2022 the AppCore .NET project.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace AppCore.Extensions.DependencyInjection
{
    internal class ServiceCollectionServiceProvider : IServiceProvider
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

        private IEnumerable<object> GetServices(Type serviceType)
        {
            object ServiceFactory(ServiceDescriptor serviceDescriptor)
            {
                object instance = serviceDescriptor.ImplementationInstance;
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
                                      || serviceType.IsGenericType
                                      && sd.ServiceType == serviceType.GetGenericTypeDefinition())
                            .Select(ServiceFactory);
        }

        public object? GetService(Type serviceType)
        {
            if (serviceType == typeof(IServiceProvider))
                return this;

            if (_additionalServices.TryGetValue(serviceType, out object? instance))
                return instance;

            if (serviceType.IsGenericType && serviceType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
            {
                serviceType = serviceType.GenericTypeArguments[0];
                var instances = (IList) System.Activator.CreateInstance(typeof(List<>).MakeGenericType(serviceType))!;
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
}
// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace AppCore.DependencyInjection
{
    internal class ServiceCollectionServiceProvider : IServiceProvider
    {
        private readonly IServiceCollection _services;
        private readonly Dictionary<Type, object> _additionalServices = new Dictionary<Type, object>();

        public ServiceCollectionServiceProvider(IServiceCollection services)
        {
            _services = services;
        }

        public void AddService(Type serviceType, object instance)
        {
            _additionalServices.Add(serviceType, instance);
        }

        public object GetService(Type serviceType)
        {
            if (_additionalServices.TryGetValue(serviceType, out object instance))
                return instance;

            return _services.FirstOrDefault(
                                sd => sd.ServiceType == serviceType
                                      && sd.Lifetime == ServiceLifetime.Singleton
                                      && sd.ImplementationInstance != null)
                            ?.ImplementationInstance;
        }
    }
}
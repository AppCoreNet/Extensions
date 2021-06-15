// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace AppCore.DependencyInjection
{
    internal class ServiceCollectionServiceProvider : IServiceProvider
    {
        private readonly IServiceCollection _services;

        public ServiceCollectionServiceProvider(IServiceCollection services)
        {
            _services = services;
        }

        public object GetService(Type serviceType)
        {
            return _services.FirstOrDefault(
                                sd => sd.ServiceType == serviceType
                                      && sd.Lifetime == ServiceLifetime.Singleton
                                      && sd.ImplementationInstance != null)
                            ?.ImplementationInstance;
        }
    }
}
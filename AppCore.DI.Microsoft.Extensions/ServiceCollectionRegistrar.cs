// Copyright 2018 the AppCore project.
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated
// documentation files (the "Software"), to deal in the Software without restriction, including without limitation
// the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and
// to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or substantial portions
// of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
// BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AppCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using MicrosoftDI = Microsoft.Extensions.DependencyInjection;

namespace AppCore.DependencyInjection
{
    /// <summary>
    /// Provides Microsoft Dependency Injection based <see cref="IServiceRegistrar"/> implementation.
    /// </summary>
    public class ServiceCollectionRegistrar : IServiceRegistrar
    {
        private readonly IServiceCollection _services;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceCollectionRegistrar"/> class.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> where services are registered.</param>
        public ServiceCollectionRegistrar(IServiceCollection services)
        {
            Ensure.Arg.NotNull(services, nameof(services));
            _services = services;
        }

        /// <inheritdoc />
        public void Register(ServiceRegistration registration)
        {
            if ((registration.Flags & ServiceRegistrationFlags.SkipIfRegistered)
                == ServiceRegistrationFlags.SkipIfRegistered)
            {
                if ((registration.Flags & ServiceRegistrationFlags.Enumerable) == ServiceRegistrationFlags.Enumerable)
                {
                    if (_services.Any(
                        sd =>
                        {
                            if (sd.ServiceType != registration.ServiceType)
                                return false;

                            if (sd.ImplementationType != null)
                            {
                                return sd.ImplementationType == registration.ImplementationType;
                            }

                            if (sd.ImplementationInstance != null)
                            {
                                return sd.ImplementationInstance.GetType() == registration.LimitType;
                            }

                            if (sd.ImplementationFactory != null)
                            {
                                return sd.ImplementationFactory.Target.GetType()
                                         .GenericTypeArguments[0]
                                       == registration.LimitType;
                            }

                            return false;
                        }))
                        return;
                }
                else
                {
                    if (_services.Any(sd => sd.ServiceType == registration.ServiceType))
                        return;
                }
            }

            ServiceDescriptor descriptor = null;

            if (registration.ImplementationFactory != null)
            {
                descriptor = ServiceDescriptor.Describe(
                    registration.ServiceType,
                    registration.ImplementationFactory,
                    ConvertLifetime(registration.Lifetime));
            }

            else if (registration.ImplementationInstance != null)
            {
                descriptor = new ServiceDescriptor(
                    registration.ServiceType,
                    registration.ImplementationInstance);
            }

            else if (registration.ImplementationType != null)
            {
                descriptor = ServiceDescriptor.Describe(
                    registration.ServiceType,
                    registration.ImplementationType,
                    ConvertLifetime(registration.Lifetime));
            }

            _services.Add(descriptor);
        }

        /// <inheritdoc />
        public void RegisterAssembly(AssemblyRegistration registration)
        {
            var scanner = new AssemblyScanner();
            IEnumerable<Type> implementationTypes =
                registration.Assemblies.SelectMany(assembly => scanner.GetTypes(assembly, registration.ServiceType));

            ServiceLifetime GetServiceLifetime(Type implementationType)
            {
                var lifetimeAttribute =
                    implementationType.GetTypeInfo()
                                      .GetCustomAttribute<ServiceLifetimeAttribute>();

                return lifetimeAttribute?.Lifetime ?? registration.DefaultLifetime;
            }

            bool isOpenGenericService = registration.ServiceType.GetTypeInfo()
                                                    .IsGenericTypeDefinition;

            foreach (Type implementationType in implementationTypes)
            {
                Type serviceType = registration.ServiceType;

                // need to register closed types with closed generic service type
                if (isOpenGenericService && !implementationType.GetTypeInfo().IsGenericTypeDefinition)
                {
                    serviceType = implementationType.GetClosedTypeOf(serviceType);
                }

                Register(
                    ServiceRegistration.Create(
                        serviceType,
                        implementationType,
                        GetServiceLifetime(implementationType),
                        registration.Flags));
            }
        }

        private MicrosoftDI.ServiceLifetime ConvertLifetime(ServiceLifetime lifetime)
        {
            MicrosoftDI.ServiceLifetime result;
            switch (lifetime)
            {
                case ServiceLifetime.Transient:
                    result = MicrosoftDI.ServiceLifetime.Transient;
                    break;
                case ServiceLifetime.Singleton:
                    result = MicrosoftDI.ServiceLifetime.Singleton;
                    break;
                case ServiceLifetime.Scoped:
                    result = MicrosoftDI.ServiceLifetime.Scoped;
                    break;
                default:
                    throw new NotImplementedException();
            }

            return result;
        }
    }
}

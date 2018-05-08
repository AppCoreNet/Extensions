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
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace AppCore.DependencyInjection.Microsoft.Extensions
{
    /// <summary>
    /// Provides Microsoft Dependency Injection based <see cref="IComponentRegistry"/> implementation.
    /// </summary>
    public class MicrosoftComponentRegistry : IComponentRegistry
    {
        private readonly IServiceCollection _services;

        /// <summary>
        /// Initializes a new instance of the <see cref="MicrosoftComponentRegistry"/> class.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> where services are registered.</param>
        public MicrosoftComponentRegistry(IServiceCollection services)
        {
            Ensure.Arg.NotNull(services, nameof(services));
            _services = services;

            services.TryAddSingleton<IContainer, MicrosoftContainer>();
            services.TryAddScoped<IContainerScopeFactory, MicrosoftContainerScopeFactory>();
        }

        /// <inheritdoc />
        public void Register(ComponentRegistration registration)
        {
            if ((registration.Flags & ComponentRegistrationFlags.SkipIfRegistered)
                == ComponentRegistrationFlags.SkipIfRegistered)
            {
                if ((registration.Flags & ComponentRegistrationFlags.Enumerable) == ComponentRegistrationFlags.Enumerable)
                {
                    if (_services.Any(
                        sd =>
                        {
                            if (sd.ServiceType != registration.ContractType)
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
                                Type implementationFactoryType = sd.ImplementationFactory.Target.GetType();
                                if (implementationFactoryType == typeof(ImplementationFactoryWrapper))
                                {
                                    return ((ImplementationFactoryWrapper) sd.ImplementationFactory.Target).LimitType
                                           == registration.LimitType;
                                }

                                return implementationFactoryType.GetTypeInfo().IsGenericType
                                       && implementationFactoryType.GenericTypeArguments[0]
                                       == registration.LimitType;
                            }

                            return false;
                        }))
                        return;
                }
                else
                {
                    if (_services.Any(sd => sd.ServiceType == registration.ContractType))
                        return;
                }
            }

            ServiceDescriptor descriptor = null;

            if (registration.ImplementationFactory != null)
            {
                var factoryWrapper = new ImplementationFactoryWrapper(
                    registration.ImplementationFactory,
                    registration.LimitType);

                descriptor = ServiceDescriptor.Describe(
                    registration.ContractType,
                    factoryWrapper.CreateInstance,
                    ConvertLifetime(registration.Lifetime));
            }

            else if (registration.ImplementationInstance != null)
            {
                descriptor = new ServiceDescriptor(
                    registration.ContractType,
                    registration.ImplementationInstance);
            }

            else if (registration.ImplementationType != null)
            {
                descriptor = ServiceDescriptor.Describe(
                    registration.ContractType,
                    registration.ImplementationType,
                    ConvertLifetime(registration.Lifetime));
            }

            _services.Add(descriptor);
        }

        /// <inheritdoc />
        public void RegisterAssembly(ComponentAssemblyRegistration registration)
        {
            var scanner = new AssemblyScanner();
            IEnumerable<Type> implementationTypes =
                registration.Assemblies.SelectMany(assembly => scanner.GetTypes(assembly, registration.ContractType));

            ComponentLifetime GetServiceLifetime(Type implementationType)
            {
                var lifetimeAttribute =
                    implementationType.GetTypeInfo()
                                      .GetCustomAttribute<LifetimeAttribute>();

                return lifetimeAttribute?.Lifetime ?? registration.DefaultLifetime;
            }

            bool isOpenGenericService = registration.ContractType.GetTypeInfo()
                                                    .IsGenericTypeDefinition;

            foreach (Type implementationType in implementationTypes)
            {
                Type serviceType = registration.ContractType;

                // need to register closed types with closed generic service type
                if (isOpenGenericService && !implementationType.GetTypeInfo().IsGenericTypeDefinition)
                {
                    serviceType = implementationType.GetClosedTypeOf(serviceType);
                }

                Register(
                    ComponentRegistration.Create(
                        serviceType,
                        implementationType,
                        GetServiceLifetime(implementationType),
                        registration.Flags));
            }
        }

        private ServiceLifetime ConvertLifetime(ComponentLifetime lifetime)
        {
            ServiceLifetime result;
            switch (lifetime)
            {
                case ComponentLifetime.Transient:
                    result = ServiceLifetime.Transient;
                    break;
                case ComponentLifetime.Singleton:
                    result = ServiceLifetime.Singleton;
                    break;
                case ComponentLifetime.Scoped:
                    result = ServiceLifetime.Scoped;
                    break;
                default:
                    throw new NotImplementedException();
            }

            return result;
        }
    }
}

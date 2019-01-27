// Licensed under the MIT License.
// Copyright (c) 2018 the AppCore .NET project.

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
        private readonly List<Func<IEnumerable<ComponentRegistration>>> _registrationCallbacks =
            new List<Func<IEnumerable<ComponentRegistration>>>();

        /// <summary>
        /// Initializes a new instance of the <see cref="MicrosoftComponentRegistry"/> class.
        /// </summary>
        public MicrosoftComponentRegistry()
        {
        }

        private void Register(IServiceCollection services, ComponentRegistration registration)
        {
            if ((registration.Flags & ComponentRegistrationFlags.IfNoneRegistered)
                == ComponentRegistrationFlags.IfNoneRegistered)
            {
                if ((registration.Flags & ComponentRegistrationFlags.IfNotRegistered) == ComponentRegistrationFlags.IfNotRegistered)
                {
                    if (services.Any(
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
                    if (services.Any(sd => sd.ServiceType == registration.ContractType))
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

            services.Add(descriptor);
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

        /// <inheritdoc />
        public void RegisterCallback(Func<IEnumerable<ComponentRegistration>> registrationCallback)
        {
            Ensure.Arg.NotNull(registrationCallback, nameof(registrationCallback));
            _registrationCallbacks.Add(registrationCallback);
        }

        /// <summary>
        /// Registers all components with the specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> where to register components.</param>
        public void RegisterComponents(IServiceCollection services)
        {
            Ensure.Arg.NotNull(services, nameof(services));

            services.TryAddTransient<IContainer, MicrosoftContainer>();
            services.TryAddScoped<IContainerScopeFactory, MicrosoftContainerScopeFactory>();

            foreach (ComponentRegistration registration in _registrationCallbacks.SelectMany(c => c()))
            {
                Register(services, registration);
            }
        }
    }
}

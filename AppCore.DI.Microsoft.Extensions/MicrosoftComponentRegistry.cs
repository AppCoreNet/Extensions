// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using System;
using System.Collections.Generic;
using System.Linq;
using AppCore.DependencyInjection.Activator;
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
        public MicrosoftComponentRegistry(IServiceCollection services)
        {
            Ensure.Arg.NotNull(services, nameof(services));

            services.TryAddTransient<IContainer, MicrosoftContainer>();
            services.TryAddScoped<IContainerScopeFactory, MicrosoftContainerScopeFactory>();
            services.TryAddTransient<IActivator, ContainerActivator>();

            _services = services;
        }

        private interface IFactoryWrapper
        {
            Func<IServiceProvider, object> GetFactory();
        }

        private class FactoryWrapper<TImplementation> : IFactoryWrapper
            where TImplementation : class
        {
            private readonly Func<IServiceProvider, TImplementation> _factory;

            public FactoryWrapper(IFactory<TImplementation> factory)
            {
                _factory = sp => factory.Create(sp.GetRequiredService<IContainer>());
            }

            public Func<IServiceProvider, object> GetFactory()
            {
                return _factory;
            }
        }

        private ServiceDescriptor CreateDescriptor(ComponentRegistration registration)
        {
            ServiceLifetime GetLifetime(ComponentLifetime lifetime)
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

            if (registration.ImplementationType != null)
            {
                return new ServiceDescriptor(
                    registration.ContractType,
                    registration.ImplementationType,
                    GetLifetime(registration.Lifetime));
            }

            if (registration.ImplementationInstance != null)
            {
                return new ServiceDescriptor(registration.ContractType, registration.ImplementationInstance);
            }

            if (registration.ImplementationFactory != null)
            {
                Type factoryWrapperType = typeof(FactoryWrapper<>).MakeGenericType(registration.GetImplementationType());
                var factoryWrapper = (IFactoryWrapper) System.Activator.CreateInstance(
                    factoryWrapperType,
                    registration.ImplementationFactory);

                return new ServiceDescriptor(
                    registration.ContractType,
                    factoryWrapper.GetFactory(),
                    GetLifetime(registration.Lifetime));
            }

            throw new InvalidOperationException("Internal error");
        }

        /// <inheritdoc />
        public IComponentRegistry Add(IEnumerable<ComponentRegistration> registrations)
        {
            Ensure.Arg.NotNull(registrations, nameof(registrations));
            _services.Add(registrations.Select(CreateDescriptor));
            return this;
        }

        /// <inheritdoc />
        public IComponentRegistry TryAdd(IEnumerable<ComponentRegistration> registrations)
        {
            Ensure.Arg.NotNull(registrations, nameof(registrations));
            _services.TryAdd(registrations.Select(CreateDescriptor));
            return this;
        }

        /// <inheritdoc />
        public IComponentRegistry TryAddEnumerable(IEnumerable<ComponentRegistration> registrations)
        {
            Ensure.Arg.NotNull(registrations, nameof(registrations));
            _services.TryAddEnumerable(registrations.Select(CreateDescriptor));
            return this;
        }
    }
}

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
using Autofac;
using Autofac.Builder;
using Autofac.Core;

namespace AppCore.DependencyInjection
{
    /// <summary>
    /// Provides Autofac based <see cref="IServiceRegistrar"/> implementation.
    /// </summary>
    public class AutofacServiceRegistrar : IServiceRegistrar
    {
        private readonly ContainerBuilder _builder;

        /// <summary>
        /// Initializes a new instance of the <see cref="AutofacServiceRegistrar"/> class.
        /// </summary>
        /// <param name="builder">The <see cref="ContainerBuilder"/>.</param>
        public AutofacServiceRegistrar(ContainerBuilder builder)
        {
            Ensure.Arg.NotNull(builder, nameof(builder));

            _builder = builder;

            _builder.RegisterType<AutofacServiceProvider>()
                    .As<IServiceProvider>()
                    .InstancePerLifetimeScope()
                    .IfNotRegistered(typeof(IServiceProvider));
        }

        /// <inheritdoc />
        public void Register(ServiceRegistration registration)
        {
            if (registration.ImplementationFactory != null)
            {
                IRegistrationBuilder<object, SimpleActivatorData, SingleRegistrationStyle> rb =
                    RegistrationBuilder.ForDelegate(
                        registration.LimitType,
                        (ctxt, parameters) => registration.ImplementationFactory(ctxt.Resolve<IServiceProvider>()));

                rb.RegistrationData.DeferredCallback = _builder.RegisterCallback(
                    cr => RegistrationBuilder.RegisterSingleComponent(cr, rb));

                rb.As(registration.ServiceType);
                ApplyLifetime(rb, registration.Lifetime);
                ApplyFlags(rb, registration.Flags, registration.ServiceType, registration.LimitType);
            }

            else if (registration.ImplementationInstance != null)
            {
                IRegistrationBuilder<object, SimpleActivatorData, SingleRegistrationStyle> rb =
                    _builder.RegisterInstance(registration.ImplementationInstance);

                rb.As(registration.ServiceType);
                ApplyLifetime(rb, registration.Lifetime);
                ApplyFlags(rb, registration.Flags, registration.ServiceType, registration.LimitType);
            }

            else if (registration.ImplementationType != null)
            {
                if (registration.ImplementationType.GetTypeInfo()
                                .IsGenericTypeDefinition)
                {
                    IRegistrationBuilder<object, ReflectionActivatorData, DynamicRegistrationStyle> rb =
                        _builder.RegisterGeneric(registration.ImplementationType);

                    rb.As(registration.ServiceType);
                    ApplyLifetime(rb, registration.Lifetime);
                    ApplyFlags(rb, registration.Flags, registration.ServiceType, registration.LimitType);
                }
                else
                {
                    IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle> rb =
                        _builder.RegisterType(registration.ImplementationType);

                    rb.As(registration.ServiceType);
                    ApplyLifetime(rb, registration.Lifetime);
                    ApplyFlags(rb, registration.Flags, registration.ServiceType, registration.LimitType);
                }
            }
        }

        private void ApplyLifetime<TLimit, TActivatorData, TRegistrationStyle>(
            IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> rb,
            ServiceLifetime lifetime)
        {
            switch (lifetime)
            {
                case ServiceLifetime.Transient:
                    rb.InstancePerDependency();
                    break;
                case ServiceLifetime.Singleton:
                    rb.SingleInstance();
                    break;
                case ServiceLifetime.Scoped:
                    rb.InstancePerLifetimeScope();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void ApplyFlags<TLimit, TActivatorData, TRegistrationStyle>(
            IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> rb,
            ServiceRegistrationFlags flags, Type serviceType, Type limitType)
        {
            if ((flags & ServiceRegistrationFlags.SkipIfRegistered)
                == ServiceRegistrationFlags.SkipIfRegistered)
            {
                rb.OnlyIf(
                    r =>
                    {
                        Type resolvedServiceType = serviceType;
                        Type resolvedLimitType = limitType;

                        if (serviceType.GetTypeInfo()
                                       .IsGenericTypeDefinition)
                        {
                            // in case of open generic services we need to check against closed
                            // generic service type
                            resolvedServiceType = serviceType.MakeGenericType(typeof(object));
                            resolvedLimitType = limitType.MakeGenericType(typeof(object));
                        }

                        IEnumerable<IInstanceActivator> instanceActivators =
                            r.RegistrationsFor(new TypedService(resolvedServiceType))
                             .Select(cr => cr.Activator);

                        if ((flags & ServiceRegistrationFlags.Enumerable)
                            == ServiceRegistrationFlags.Enumerable)
                        {
                            return instanceActivators.All(a => a.LimitType != resolvedLimitType);
                        }

                        return !instanceActivators.Any();
                    });
            }
        }

        /// <inheritdoc />
        public void RegisterAssembly(AssemblyRegistration registration)
        {
            // Autofac doesnt support scanning for open generic types so we have to
            // do it on our own :(

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
    }
}
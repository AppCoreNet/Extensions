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

namespace AppCore.DependencyInjection.Autofac
{
    /// <summary>
    /// Provides Autofac based <see cref="IComponentRegistry"/> implementation.
    /// </summary>
    public class AutofacComponentRegistry : IComponentRegistry
    {
        private readonly ContainerBuilder _builder;

        /// <summary>
        /// Initializes a new instance of the <see cref="AutofacComponentRegistry"/> class.
        /// </summary>
        /// <param name="builder">The <see cref="ContainerBuilder"/>.</param>
        public AutofacComponentRegistry(ContainerBuilder builder)
        {
            Ensure.Arg.NotNull(builder, nameof(builder));

            _builder = builder;

            _builder.RegisterType<AutofacContainer>()
                    .As<IContainer>()
                    .IfNotRegistered(typeof(IContainer));

            _builder.RegisterType<AutofacContainerScopeFactory>()
                    .As<IContainerScopeFactory>()
                    .IfNotRegistered(typeof(IContainerScopeFactory));
        }

        /// <inheritdoc />
        public void Register(ComponentRegistration registration)
        {
            if (registration.ImplementationFactory != null)
            {
                IRegistrationBuilder<object, SimpleActivatorData, SingleRegistrationStyle> rb =
                    RegistrationBuilder.ForDelegate(
                        registration.LimitType,
                        (ctxt, parameters) => registration.ImplementationFactory(ctxt.Resolve<IContainer>()));

                rb.RegistrationData.DeferredCallback = _builder.RegisterCallback(
                    cr => RegistrationBuilder.RegisterSingleComponent(cr, rb));

                rb.As(registration.ContractType);
                ApplyLifetime(rb, registration.Lifetime);
                ApplyFlags(rb, registration.Flags, registration.ContractType, registration.LimitType);
            }

            else if (registration.ImplementationInstance != null)
            {
                IRegistrationBuilder<object, SimpleActivatorData, SingleRegistrationStyle> rb =
                    _builder.RegisterInstance(registration.ImplementationInstance);

                rb.As(registration.ContractType);
                ApplyLifetime(rb, registration.Lifetime);
                ApplyFlags(rb, registration.Flags, registration.ContractType, registration.LimitType);
            }

            else if (registration.ImplementationType != null)
            {
                if (registration.ImplementationType.GetTypeInfo()
                                .IsGenericTypeDefinition)
                {
                    IRegistrationBuilder<object, ReflectionActivatorData, DynamicRegistrationStyle> rb =
                        _builder.RegisterGeneric(registration.ImplementationType);

                    rb.As(registration.ContractType);
                    ApplyLifetime(rb, registration.Lifetime);
                    ApplyFlags(rb, registration.Flags, registration.ContractType, registration.LimitType);
                }
                else
                {
                    IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle> rb =
                        _builder.RegisterType(registration.ImplementationType);

                    rb.As(registration.ContractType);
                    ApplyLifetime(rb, registration.Lifetime);
                    ApplyFlags(rb, registration.Flags, registration.ContractType, registration.LimitType);
                }
            }
        }

        private void ApplyLifetime<TLimit, TActivatorData, TRegistrationStyle>(
            IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> rb,
            ComponentLifetime lifetime)
        {
            switch (lifetime)
            {
                case ComponentLifetime.Transient:
                    rb.InstancePerDependency();
                    break;
                case ComponentLifetime.Singleton:
                    rb.SingleInstance();
                    break;
                case ComponentLifetime.Scoped:
                    rb.InstancePerLifetimeScope();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void ApplyFlags<TLimit, TActivatorData, TRegistrationStyle>(
            IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> rb,
            ComponentRegistrationFlags flags, Type serviceType, Type limitType)
        {
            if ((flags & ComponentRegistrationFlags.SkipIfRegistered)
                == ComponentRegistrationFlags.SkipIfRegistered)
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

                        if ((flags & ComponentRegistrationFlags.Enumerable)
                            == ComponentRegistrationFlags.Enumerable)
                        {
                            return instanceActivators.All(a => a.LimitType != resolvedLimitType);
                        }

                        return !instanceActivators.Any();
                    });
            }
        }

        /// <inheritdoc />
        public void RegisterAssembly(ComponentAssemblyRegistration registration)
        {
            // Autofac doesnt support scanning for open generic types so we have to
            // do it on our own :(

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
    }
}
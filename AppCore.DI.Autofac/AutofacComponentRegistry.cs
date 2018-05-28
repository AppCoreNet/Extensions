// Licensed under the MIT License.
// Copyright (c) 2018 the AppCore .NET project.

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
        private readonly List<Func<IEnumerable<ComponentRegistration>>> _registrationCallbacks =
            new List<Func<IEnumerable<ComponentRegistration>>>();

        /// <summary>
        /// Initializes a new instance of the <see cref="AutofacComponentRegistry"/> class.
        /// </summary>
        public AutofacComponentRegistry()
        {
        }

        private void Register(ContainerBuilder builder, ComponentRegistration registration)
        {
            if (registration.ImplementationFactory != null)
            {
                IRegistrationBuilder<object, SimpleActivatorData, SingleRegistrationStyle> rb =
                    RegistrationBuilder.ForDelegate(
                        registration.LimitType,
                        (ctxt, parameters) => registration.ImplementationFactory(ctxt.Resolve<IContainer>()));

                rb.RegistrationData.DeferredCallback = builder.RegisterCallback(
                    cr => RegistrationBuilder.RegisterSingleComponent(cr, rb));

                rb.As(registration.ContractType);
                ApplyLifetime(rb, registration.Lifetime);
                ApplyFlags(rb, registration.Flags, registration.ContractType, registration.LimitType);
            }

            else if (registration.ImplementationInstance != null)
            {
                IRegistrationBuilder<object, SimpleActivatorData, SingleRegistrationStyle> rb =
                    builder.RegisterInstance(registration.ImplementationInstance);

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
                        builder.RegisterGeneric(registration.ImplementationType);

                    rb.As(registration.ContractType);
                    ApplyLifetime(rb, registration.Lifetime);
                    ApplyFlags(rb, registration.Flags, registration.ContractType, registration.LimitType);
                }
                else
                {
                    IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle> rb =
                        builder.RegisterType(registration.ImplementationType);

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
            if ((flags & ComponentRegistrationFlags.IfNoneRegistered)
                == ComponentRegistrationFlags.IfNoneRegistered)
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

                        if ((flags & ComponentRegistrationFlags.IfNotRegistered)
                            == ComponentRegistrationFlags.IfNotRegistered)
                        {
                            return instanceActivators.All(a => a.LimitType != resolvedLimitType);
                        }

                        return !instanceActivators.Any();
                    });
            }
        }

        /// <inheritdoc />
        public void RegisterCallback(Func<IEnumerable<ComponentRegistration>> registrationCallback)
        {
            Ensure.Arg.NotNull(registrationCallback, nameof(registrationCallback));
            _registrationCallbacks.Add(registrationCallback);
        }

        public void RegisterComponents(ContainerBuilder builder)
        {
            Ensure.Arg.NotNull(builder, nameof(builder));

            builder.RegisterType<AutofacContainer>()
                   .As<IContainer>()
                   .IfNotRegistered(typeof(IContainer));

            builder.RegisterType<AutofacContainerScopeFactory>()
                   .As<IContainerScopeFactory>()
                   .IfNotRegistered(typeof(IContainerScopeFactory));

            foreach (ComponentRegistration registration in _registrationCallbacks.SelectMany(c => c()))
            {
                Register(builder, registration);
            }
        }
    }
}
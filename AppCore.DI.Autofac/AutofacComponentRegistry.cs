// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

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

            builder.RegisterType<AutofacContainer>()
                   .As<IContainer>()
                   .IfNotRegistered(typeof(IContainer));

            builder.RegisterType<AutofacContainerScopeFactory>()
                   .As<IContainerScopeFactory>()
                   .IfNotRegistered(typeof(IContainerScopeFactory));

            _builder = builder;
        }

        /// <inheritdoc />
        public IComponentRegistry Add(IEnumerable<ComponentRegistration> registrations)
        {
            Ensure.Arg.NotNull(registrations, nameof(registrations));

            foreach (ComponentRegistration registration in registrations)
            {
                Register(registration);
            }

            return this;
        }

        /// <inheritdoc />
        public IComponentRegistry TryAdd(IEnumerable<ComponentRegistration> registrations)
        {
            Ensure.Arg.NotNull(registrations, nameof(registrations));

            foreach (ComponentRegistration registration in registrations)
            {
                Register(registration, false, true);
            }

            return this;
        }

        /// <inheritdoc />
        public IComponentRegistry TryAddEnumerable(IEnumerable<ComponentRegistration> registrations)
        {
            Ensure.Arg.NotNull(registrations, nameof(registrations));

            foreach (ComponentRegistration registration in registrations)
            {
                Register(registration, true, false);
            }

            return this;
        }

        private void Register(
            ComponentRegistration registration,
            bool skipIfNotPresent = false,
            bool skipIfNonePresent = false)
        {
            Type implementationType = registration.GetImplementationType();

            if (registration.ImplementationFactory != null)
            {
                IRegistrationBuilder<object, SimpleActivatorData, SingleRegistrationStyle> rb =
                    RegistrationBuilder.ForDelegate(
                        implementationType,
                        (context, _) =>
                            registration.ImplementationFactory.Create(context.Resolve<IContainer>()));

                rb.RegistrationData.DeferredCallback = _builder.RegisterCallback(
                    cr => RegistrationBuilder.RegisterSingleComponent(cr, rb));

                rb.As(registration.ContractType);
                ApplyLifetime(rb, registration.Lifetime);
                ApplyFlags(rb, registration.ContractType, implementationType, skipIfNotPresent, skipIfNonePresent);
            }

            else if (registration.ImplementationInstance != null)
            {
                IRegistrationBuilder<object, SimpleActivatorData, SingleRegistrationStyle> rb =
                    _builder.RegisterInstance(registration.ImplementationInstance);

                rb.As(registration.ContractType);
                ApplyLifetime(rb, registration.Lifetime);
                ApplyFlags(rb, registration.ContractType, implementationType, skipIfNotPresent, skipIfNonePresent);
            }

            else if (registration.ImplementationType != null)
            {
                if (registration.ImplementationType.IsGenericTypeDefinition)
                {
                    IRegistrationBuilder<object, ReflectionActivatorData, DynamicRegistrationStyle> rb =
                        _builder.RegisterGeneric(registration.ImplementationType);

                    rb.As(registration.ContractType);
                    ApplyLifetime(rb, registration.Lifetime);
                    ApplyFlags(rb, registration.ContractType, implementationType, skipIfNotPresent, skipIfNonePresent);
                }
                else
                {
                    IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle> rb =
                        _builder.RegisterType(registration.ImplementationType);

                    rb.As(registration.ContractType);
                    ApplyLifetime(rb, registration.Lifetime);
                    ApplyFlags(rb, registration.ContractType, implementationType, skipIfNotPresent, skipIfNonePresent);
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
            Type serviceType,
            Type limitType,
            bool skipIfNotPresent = false,
            bool skipIfNonePresent = false)
        {
            if (!skipIfNotPresent && !skipIfNonePresent)
                return;

            rb.OnlyIf(
                r =>
                {
                    // in case of open generic services we need to check against closed
                    // generic service type. See https://github.com/autofac/Autofac/issues/958
                    serviceType = ConstructOpenGenericType(serviceType);

                    IEnumerable<IInstanceActivator> instanceActivators =
                        r.RegistrationsFor(new TypedService(serviceType))
                         .Select(cr => cr.Activator);

                    if (skipIfNotPresent)
                    {
                        limitType = ConstructOpenGenericType(limitType);
                        return instanceActivators.All(a => a.LimitType != limitType);
                    }

                    return !instanceActivators.Any();
                });
        }

        private static Type ConstructOpenGenericType(Type serviceType)
        {
            TypeInfo serviceTypeInfo = serviceType.GetTypeInfo();
            if (serviceTypeInfo.IsGenericTypeDefinition)
            {
                Type[] serviceTypeParameters = serviceTypeInfo.GenericTypeParameters;

                var genericTypeArguments = new Dictionary<Type,Type>();
                foreach (Type serviceTypeParameter in serviceTypeParameters)
                {
                    genericTypeArguments.Add(
                        serviceTypeParameter,
                        serviceTypeParameter
                            .GetTypeInfo()
                            .GetGenericParameterConstraints()
                            .SingleOrDefault()?? typeof(object));
                }

                foreach (Type genericTypeArgument in genericTypeArguments.Keys.ToArray())
                {
                    TypeInfo genericTypeArgumentTypeInfo = genericTypeArguments[genericTypeArgument].GetTypeInfo();
                    if (genericTypeArgumentTypeInfo.IsGenericType && genericTypeArgumentTypeInfo.ContainsGenericParameters)
                    {
                        var genericTypeParameters =
                            new Type[genericTypeArgumentTypeInfo.GenericTypeArguments.Length];

                        for (int i = 0; i < genericTypeArgumentTypeInfo.GenericTypeArguments.Length; i++)
                        {
                            if (genericTypeArgumentTypeInfo.GenericTypeArguments[i]
                                                           .IsGenericParameter)
                            {
                                genericTypeParameters[i] = genericTypeArguments[genericTypeArgumentTypeInfo.GenericTypeArguments[i]];
                            }
                            else
                            {
                                genericTypeParameters[i] = typeof(object);
                            }
                        }

                        genericTypeArguments[genericTypeArgument] =
                            genericTypeArgumentTypeInfo.GetGenericTypeDefinition()
                                                       .MakeGenericType(genericTypeParameters);
                    }
                }

                serviceType = serviceType.MakeGenericType(
                    genericTypeArguments.OrderBy(kv => kv.Key.GenericParameterPosition)
                                        .Select(kv => kv.Value)
                                        .ToArray());
            }

            return serviceType;
        }
    }
}
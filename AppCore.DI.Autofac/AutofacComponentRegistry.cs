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
                        // in case of open generic services we need to check against closed
                        // generic service type
                        serviceType = ConstructOpenGenericType(serviceType);

                        IEnumerable<IInstanceActivator> instanceActivators =
                            r.RegistrationsFor(new TypedService(serviceType))
                             .Select(cr => cr.Activator);

                        if ((flags & ComponentRegistrationFlags.IfNotRegistered)
                            == ComponentRegistrationFlags.IfNotRegistered)
                        {
                            limitType = ConstructOpenGenericType(limitType);
                            return instanceActivators.All(a => a.LimitType != limitType);
                        }

                        return !instanceActivators.Any();
                    });
            }
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

                        for (var i = 0; i < genericTypeArgumentTypeInfo.GenericTypeArguments.Length; i++)
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

        /// <inheritdoc />
        public void RegisterCallback(Func<IEnumerable<ComponentRegistration>> registrationCallback)
        {
            Ensure.Arg.NotNull(registrationCallback, nameof(registrationCallback));
            _registrationCallbacks.Add(registrationCallback);
        }

        /// <summary>
        /// Registers components with the given <see cref="ContainerBuilder"/>.
        /// </summary>
        /// <param name="builder">The <see cref="ContainerBuilder"/> where components are registered.</param>
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
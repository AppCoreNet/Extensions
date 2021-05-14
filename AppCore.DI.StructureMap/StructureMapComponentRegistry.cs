// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using System;
using System.Collections.Generic;
using System.Linq;
using AppCore.Diagnostics;
using StructureMap;
using StructureMap.Graph;
using StructureMap.Pipeline;

namespace AppCore.DependencyInjection.StructureMap
{
    /// <summary>
    /// Represents StructureMap based <see cref="IComponentRegistry"/>.
    /// </summary>
    public class StructureMapComponentRegistry : IComponentRegistry
    {
        private readonly Registry _registry;

        /// <summary>
        /// Initializes a new instance of the <see cref="StructureMapComponentRegistry"/>.
        /// </summary>
        public StructureMapComponentRegistry(Registry registry)
        {
            Ensure.Arg.NotNull(registry, nameof(registry));

            registry.For<IContainer>()
                    .LifecycleIs(Lifecycles.Container)
                    .UseIfNone<StructureMapContainer>();

            registry.For<IContainerScopeFactory>()
                    .LifecycleIs(Lifecycles.Container)
                    .UseIfNone<StructureMapContainerScopeFactory>();

            registry.For<IActivator>()
                    .LifecycleIs(Lifecycles.Transient)
                    .UseIfNone<Activator>();

            _registry = registry;
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

        private void Register(ComponentRegistration registration,
                              bool skipIfNotPresent = false,
                              bool skipIfNonePresent = false)
        {
            _registry.Configure(
                pg =>
                {
                    Type implementationType = registration.GetImplementationType();

                    if (skipIfNotPresent || skipIfNonePresent)
                    {
                        if (skipIfNotPresent)
                        {
                            if (pg.HasFamily(registration.ContractType)
                                && pg.Families[registration.ContractType]
                                     .Instances.Any(i => i.ReturnedType == implementationType))
                                return;
                        }
                        else
                        {
                            if (pg.HasFamily(registration.ContractType))
                                return;
                        }
                    }

                    PluginFamily family = pg.Families[registration.ContractType];
                    Instance instance = null;

                    if (registration.ImplementationFactory != null)
                    {
                        Type instanceType = typeof(ContainerLambdaInstance<,>).MakeGenericType(
                            implementationType,
                            registration.ContractType);

                        instance = (Instance) System.Activator.CreateInstance(instanceType, registration.ImplementationFactory);
                    }

                    else if (registration.ImplementationInstance != null)
                    {
                        instance = new ObjectInstance(registration.ImplementationInstance);
                    }

                    else if (registration.ImplementationType != null)
                    {
                        instance = new ConstructorInstance(registration.ImplementationType);
                    }

                    instance.SetLifecycleTo(GetLifecycle(registration.Lifetime));
                    family.AddInstance(instance);
                });
        }

        private static ILifecycle GetLifecycle(ComponentLifetime lifetime)
        {
            switch (lifetime)
            {
                case ComponentLifetime.Transient:
                    return Lifecycles.Unique;
                case ComponentLifetime.Singleton:
                    return Lifecycles.Singleton;
                case ComponentLifetime.Scoped:
                    return Lifecycles.Container;
                default:
                    throw new ArgumentOutOfRangeException(nameof(lifetime), lifetime, null);
            }
        }
    }
}
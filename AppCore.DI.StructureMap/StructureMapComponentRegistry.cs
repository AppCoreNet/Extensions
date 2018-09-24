// Licensed under the MIT License.
// Copyright (c) 2018 the AppCore .NET project.

using System;
using System.Collections.Generic;
using System.Linq;
using AppCore.Diagnostics;
using StructureMap;
using StructureMap.Graph;
using StructureMap.Pipeline;

namespace AppCore.DependencyInjection
{
    /// <summary>
    /// Represents StructureMap based <see cref="IComponentRegistry"/>.
    /// </summary>
    public class StructureMapComponentRegistry : IComponentRegistry
    {
        private readonly List<Func<IEnumerable<ComponentRegistration>>> _registrationCallbacks =
            new List<Func<IEnumerable<ComponentRegistration>>>();

        /// <summary>
        /// Initializes a new instance of the <see cref="StructureMapComponentRegistry"/>.
        /// </summary>
        public StructureMapComponentRegistry()
        {
        }

        private void Register(Registry registry, ComponentRegistration registration)
        {
            registry.Configure(
                pg =>
                {
                    if ((registration.Flags & ComponentRegistrationFlags.IfNoneRegistered)
                        == ComponentRegistrationFlags.IfNoneRegistered)
                    {
                        if ((registration.Flags & ComponentRegistrationFlags.IfNotRegistered)
                            == ComponentRegistrationFlags.IfNotRegistered)
                        {
                            if (pg.HasFamily(registration.ContractType)
                                && pg.Families[registration.ContractType]
                                     .Instances.Any(i => i.ReturnedType == registration.LimitType))
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
                            registration.LimitType,
                            registration.ContractType);

                        instance = (Instance) Activator.CreateInstance(
                            instanceType,
                            registration.ImplementationFactory);
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

        /// <inheritdoc />
        public void RegisterCallback(Func<IEnumerable<ComponentRegistration>> registrationCallback)
        {
            Ensure.Arg.NotNull(registrationCallback, nameof(registrationCallback));
            _registrationCallbacks.Add(registrationCallback);
        }

        /// <summary>
        /// Registers all components with the specified <paramref name="registry"/>.
        /// </summary>
        /// <param name="registry">The <see cref="Registry"/> where to register components.</param>
        public void RegisterComponents(Registry registry)
        {
            Ensure.Arg.NotNull(registry, nameof(registry));

            registry.For<IContainer>()
                .LifecycleIs(Lifecycles.Container)
                .UseIfNone<StructureMapContainer>();

            registry.For<IContainerScopeFactory>()
                .LifecycleIs(Lifecycles.Container)
                .UseIfNone<StructureMapContainerScopeFactory>();

            foreach (ComponentRegistration registration in _registrationCallbacks.SelectMany(c => c()))
            {
                Register(registry, registration);
            }
        }
    }
}
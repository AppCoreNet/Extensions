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
using System.Linq.Expressions;
using System.Reflection;
using StructureMap;
using StructureMap.Graph;
using StructureMap.Pipeline;

namespace AppCore.DependencyInjection.StructureMap
{
    public class StructureMapComponentRegistry : Registry, IComponentRegistry
    {
        public StructureMapComponentRegistry()
        {
            For<IContainer>()
                .LifecycleIs(Lifecycles.Container)
                .UseIfNone<StructureMapContainer>();

            For<IContainerScopeFactory>()
                .LifecycleIs(Lifecycles.Container)
                .UseIfNone<StructureMapContainerScopeFactory>();
        }

        public void Register(ComponentRegistration registration)
        {
            Configure(
                pg =>
                {
                    if ((registration.Flags & ComponentRegistrationFlags.SkipIfRegistered)
                        == ComponentRegistrationFlags.SkipIfRegistered)
                    {
                        if ((registration.Flags & ComponentRegistrationFlags.Enumerable)
                            == ComponentRegistrationFlags.Enumerable)
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

            foreach (Type implementationType in implementationTypes)
            {
                Register(
                    ComponentRegistration.Create(
                        registration.ContractType,
                        implementationType,
                        GetServiceLifetime(implementationType),
                        registration.Flags));
            }
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
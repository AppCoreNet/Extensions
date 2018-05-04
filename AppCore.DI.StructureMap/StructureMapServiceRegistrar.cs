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
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using StructureMap;
using StructureMap.Graph;
using StructureMap.Pipeline;
using StructureMap.TypeRules;

namespace AppCore.DependencyInjection
{
    public class StructureMapServiceRegistrar : Registry, IServiceRegistrar
    {
        public StructureMapServiceRegistrar()
        {
            For<IServiceProvider>()
                .LifecycleIs(Lifecycles.Container)
                .UseIfNone<StructureMapServiceProvider>();
        }

        public void Register(ServiceRegistration registration)
        {
            Configure(
                pg =>
                {
                    if ((registration.Flags & ServiceRegistrationFlags.SkipIfRegistered)
                        == ServiceRegistrationFlags.SkipIfRegistered)
                    {
                        if ((registration.Flags & ServiceRegistrationFlags.Enumerable)
                            == ServiceRegistrationFlags.Enumerable)
                        {
                            if (pg.HasFamily(registration.ServiceType)
                                && pg.Families[registration.ServiceType]
                                     .Instances.Any(i => i.ReturnedType == registration.LimitType))
                                return;
                        }
                        else
                        {
                            if (pg.HasFamily(registration.ServiceType))
                                return;
                        }
                    }

                    PluginFamily family = pg.Families[registration.ServiceType];
                    family.SetLifecycleTo(GetLifecycle(registration.Lifetime));

                    if (registration.ImplementationFactory != null)
                    {
                        Func<IContext, object> factoryWrapper = c =>
                            registration.ImplementationFactory(c.GetInstance<IServiceProvider>());

                        Type instanceType = typeof(LambdaInstance<,>).MakeGenericType(
                            registration.LimitType,
                            registration.ServiceType);

                        var delegateType = typeof(Func<,>).MakeGenericType(typeof(IContext), registration.LimitType);
                        var parameter = Expression.Parameter(typeof(IContext));

                        var body = Expression.Convert(Expression.Call(
                            Expression.Constant(factoryWrapper.Target),
                            factoryWrapper.GetMethodInfo(),
                            Expression.Parameter(typeof(IContext))), registration.LimitType);

                        var expression = Expression.Lambda(delegateType, body, parameter);

                        family.AddInstance((Instance) Activator.CreateInstance(instanceType, expression));
                    }

                    else if (registration.ImplementationInstance != null)
                    {
                        family.AddInstance(new ObjectInstance(registration.ImplementationInstance));
                    }

                    else if (registration.ImplementationType != null)
                    {
                        family.AddType(registration.ImplementationType);
                    }
                });
        }

        public void RegisterAssembly(AssemblyRegistration registration)
        {
            throw new NotImplementedException();
        }

        private static ILifecycle GetLifecycle(ServiceLifetime lifetime)
        {
            switch (lifetime)
            {
                case ServiceLifetime.Transient:
                    return Lifecycles.Unique;
                case ServiceLifetime.Singleton:
                    return Lifecycles.Singleton;
                case ServiceLifetime.Scoped:
                    return Lifecycles.Container;
                default:
                    throw new ArgumentOutOfRangeException(nameof(lifetime), lifetime, null);
            }
        }
    }
}
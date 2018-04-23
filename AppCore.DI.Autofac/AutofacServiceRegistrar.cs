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
    public class AutofacServiceRegistrar : IServiceRegistrar
    {
        private readonly ContainerBuilder _builder;

        public AutofacServiceRegistrar(ContainerBuilder builder)
        {
            Ensure.Arg.NotNull(builder, nameof(builder));

            _builder = builder;

            _builder.RegisterType<AutofacServiceProvider>()
                    .As<IServiceProvider>()
                    .InstancePerLifetimeScope()
                    .IfNotRegistered(typeof(IServiceProvider));
        }

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

                ApplyActivationProperties(rb, registration);
            }

            else if (registration.ImplementationInstance != null)
            {
                IRegistrationBuilder<object, SimpleActivatorData, SingleRegistrationStyle> rb =
                    _builder.RegisterInstance(registration.ImplementationInstance);

                ApplyActivationProperties(rb, registration);
            }

            else if (registration.ImplementationType != null)
            {
                if (registration.ImplementationType.GetTypeInfo()
                                .IsGenericTypeDefinition)
                {
                    IRegistrationBuilder<object, ReflectionActivatorData, DynamicRegistrationStyle> rb =
                        _builder.RegisterGeneric(registration.ImplementationType);

                    ApplyActivationProperties(rb, registration);
                }
                else
                {
                    IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle> rb =
                        _builder.RegisterType(registration.ImplementationType);

                    ApplyActivationProperties(rb, registration);
                }
            }
        }

        private void ApplyActivationProperties<TLimit, TActivatorData, TRegistrationStyle>(
            IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> rb,
            ServiceRegistration registration)
        {
            rb.As(registration.ServiceType);

            switch (registration.Lifetime)
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

            if ((registration.Flags & ServiceRegistrationFlags.SkipIfRegistered)
                == ServiceRegistrationFlags.SkipIfRegistered)
            {
                if ((registration.Flags & ServiceRegistrationFlags.Enumerable)
                    == ServiceRegistrationFlags.Enumerable)
                {
                    rb.OnlyIf(
                        r =>
                        {
                            IEnumerable<IInstanceActivator> instanceActivators =
                                r.RegistrationsFor(new TypedService(registration.ServiceType))
                                 .Select(cr => cr.Activator);

                            return instanceActivators.All(a => a.LimitType != registration.LimitType);
                        });
                }
                else
                {
                    rb.IfNotRegistered(registration.ServiceType);
                }
            }
        }
    }
}
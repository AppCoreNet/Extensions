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
using AppCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using MicrosoftDI = Microsoft.Extensions.DependencyInjection;

namespace AppCore.DependencyInjection
{
    public class ServiceCollectionRegistrar : IServiceRegistrar
    {
        private readonly IServiceCollection _services;

        public ServiceCollectionRegistrar(IServiceCollection services)
        {
            Ensure.Arg.NotNull(services, nameof(services));
            _services = services;
        }

        public void Register(ServiceRegistration registration)
        {
            if ((registration.Flags & ServiceRegistrationFlags.SkipIfRegistered)
                == ServiceRegistrationFlags.SkipIfRegistered)
            {
                if ((registration.Flags & ServiceRegistrationFlags.Enumerable) == ServiceRegistrationFlags.Enumerable)
                {
                    if (_services.Any(
                        sd =>
                        {
                            if (sd.ServiceType != registration.ServiceType)
                                return false;

                            if (sd.ImplementationType != null)
                            {
                                return sd.ImplementationType == registration.ImplementationType;
                            }

                            if (sd.ImplementationInstance != null)
                            {
                                return sd.ImplementationInstance.GetType() == registration.LimitType;
                            }

                            if (sd.ImplementationFactory != null)
                            {
                                return sd.ImplementationFactory.Target.GetType()
                                         .GenericTypeArguments[0]
                                       == registration.LimitType;
                            }

                            return false;
                        }))
                        return;
                }
                else
                {
                    if (_services.Any(sd => sd.ServiceType == registration.ServiceType))
                        return;
                }
            }

            ServiceDescriptor descriptor = null;

            if (registration.ImplementationFactory != null)
            {
                descriptor = ServiceDescriptor.Describe(
                    registration.ServiceType,
                    registration.ImplementationFactory,
                    GetLifetime(registration.Lifetime));
            }

            else if (registration.ImplementationInstance != null)
            {
                descriptor = new ServiceDescriptor(
                    registration.ServiceType,
                    registration.ImplementationInstance);
            }

            else if (registration.ImplementationType != null)
            {
                descriptor = ServiceDescriptor.Describe(
                    registration.ServiceType,
                    registration.ImplementationType,
                    GetLifetime(registration.Lifetime));
            }

            _services.Add(descriptor);
        }

        private MicrosoftDI.ServiceLifetime GetLifetime(ServiceLifetime lifetime)
        {
            MicrosoftDI.ServiceLifetime result;
            switch (lifetime)
            {
                case ServiceLifetime.Transient:
                    result = MicrosoftDI.ServiceLifetime.Transient;
                    break;
                case ServiceLifetime.Singleton:
                    result = MicrosoftDI.ServiceLifetime.Singleton;
                    break;
                case ServiceLifetime.Scoped:
                    result = MicrosoftDI.ServiceLifetime.Scoped;
                    break;
                default:
                    throw new NotImplementedException();
            }

            return result;
        }
    }
}

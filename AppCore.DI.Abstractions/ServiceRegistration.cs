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
using AppCore.Diagnostics;

namespace AppCore.DependencyInjection
{
    /// <summary>
    /// Represents parameters for registering a service.
    /// </summary>
    /// <seealso cref="IServiceRegistrar"/>
    public readonly struct ServiceRegistration
    {
        /// <summary>
        /// Gets the type of service which is registered.
        /// </summary>
        public Type ServiceType { get; }

        /// <summary>
        /// Gets the most known type of the service.
        /// </summary>
        public Type LimitType { get; }

        /// <summary>
        /// Gets the <see cref="Type"/> which is instantiated.
        /// </summary>
        public Type ImplementationType { get; }

        /// <summary>
        /// Gets the factory method used to instantiate the service.
        /// </summary>
        public Func<IServiceProvider, object> ImplementationFactory { get; }

        /// <summary>
        /// Gets the singleton instance of the service.
        /// </summary>
        public object ImplementationInstance { get; }

        /// <summary>
        /// Gets the lifetime of the service.
        /// </summary>
        public ServiceLifetime Lifetime { get; }

        /// <summary>
        /// Gets service registration flags.
        /// </summary>
        public ServiceRegistrationFlags Flags { get; }

        private ServiceRegistration(
            Type serviceType,
            Type implementationType,
            ServiceLifetime lifetime,
            ServiceRegistrationFlags flags)
        {
            ServiceType = serviceType;
            LimitType = implementationType;
            ImplementationType = implementationType;
            ImplementationFactory = null;
            ImplementationInstance = null;
            Lifetime = lifetime;
            Flags = flags;
        }

        private ServiceRegistration(
            Type serviceType,
            Type limitType,
            Func<IServiceProvider, object> implementationFactory,
            ServiceLifetime lifetime,
            ServiceRegistrationFlags flags)
        {
            ServiceType = serviceType;
            LimitType = limitType;
            ImplementationType = null;
            ImplementationFactory = implementationFactory;
            ImplementationInstance = null;
            Lifetime = lifetime;
            Flags = flags;
        }

        private ServiceRegistration(
            Type serviceType,
            object implementationInstance,
            ServiceRegistrationFlags flags)
        {
            ServiceType = serviceType;
            LimitType = implementationInstance.GetType();
            ImplementationType = null;
            ImplementationFactory = null;
            ImplementationInstance = implementationInstance;
            Lifetime = ServiceLifetime.Singleton;
            Flags = flags;
        }

        public static ServiceRegistration Create(
            Type serviceType,
            Type implementationType,
            ServiceLifetime lifetime,
            ServiceRegistrationFlags flags = ServiceRegistrationFlags.None)
        {
            Ensure.Arg.NotNull(serviceType, nameof(serviceType));
            Ensure.Arg.NotNull(implementationType, nameof(implementationType));
            Ensure.Arg.OfType(implementationType, serviceType, nameof(implementationType));

            return new ServiceRegistration(serviceType, implementationType, lifetime, flags);
        }

        public static ServiceRegistration Create<T>(
            Type serviceType,
            Func<IServiceProvider, T> implementationFactory,
            ServiceLifetime lifetime,
            ServiceRegistrationFlags flags = ServiceRegistrationFlags.None)
        {
            Ensure.Arg.NotNull(serviceType, nameof(serviceType));
            Ensure.Arg.NotNull(implementationFactory, nameof(implementationFactory));
            Ensure.Arg.OfType(typeof(T), serviceType, nameof(implementationFactory));

            return new ServiceRegistration(serviceType, typeof(T), sp => implementationFactory(sp), lifetime, flags);
        }

        public static ServiceRegistration Create<T>(
            Type serviceType,
            T implementationInstance,
            ServiceRegistrationFlags flags = ServiceRegistrationFlags.None)
        {
            Ensure.Arg.NotNull(serviceType, nameof(serviceType));
            Ensure.Arg.NotNull(implementationInstance, nameof(implementationInstance));
            Ensure.Arg.OfType(typeof(T), serviceType, nameof(implementationInstance));

            return new ServiceRegistration(serviceType, implementationInstance, flags);
        }
    }
}

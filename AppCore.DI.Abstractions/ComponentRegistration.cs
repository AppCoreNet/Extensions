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
    /// <seealso cref="IComponentRegistry"/>
    public readonly struct ComponentRegistration
    {
        /// <summary>
        /// Gets the type of component which is registered.
        /// </summary>
        public Type ContractType { get; }

        /// <summary>
        /// Gets the most known type of the component.
        /// </summary>
        public Type LimitType { get; }

        /// <summary>
        /// Gets the <see cref="Type"/> which is instantiated.
        /// </summary>
        public Type ImplementationType { get; }

        /// <summary>
        /// Gets the factory method used to instantiate the component.
        /// </summary>
        public Func<IContainer, object> ImplementationFactory { get; }

        /// <summary>
        /// Gets the singleton instance of the component.
        /// </summary>
        public object ImplementationInstance { get; }

        /// <summary>
        /// Gets the lifetime of the component.
        /// </summary>
        public ComponentLifetime Lifetime { get; }

        /// <summary>
        /// Gets component registration flags.
        /// </summary>
        public ComponentRegistrationFlags Flags { get; }

        private ComponentRegistration(
            Type contractType,
            Type implementationType,
            ComponentLifetime lifetime,
            ComponentRegistrationFlags flags)
        {
            ContractType = contractType;
            LimitType = implementationType;
            ImplementationType = implementationType;
            ImplementationFactory = null;
            ImplementationInstance = null;
            Lifetime = lifetime;
            Flags = flags;
        }

        private ComponentRegistration(
            Type contractType,
            Type limitType,
            Func<IContainer, object> implementationFactory,
            ComponentLifetime lifetime,
            ComponentRegistrationFlags flags)
        {
            ContractType = contractType;
            LimitType = limitType;
            ImplementationType = null;
            ImplementationFactory = implementationFactory;
            ImplementationInstance = null;
            Lifetime = lifetime;
            Flags = flags;
        }

        private ComponentRegistration(
            Type contractType,
            object implementationInstance,
            ComponentRegistrationFlags flags)
        {
            ContractType = contractType;
            LimitType = implementationInstance.GetType();
            ImplementationType = null;
            ImplementationFactory = null;
            ImplementationInstance = implementationInstance;
            Lifetime = ComponentLifetime.Singleton;
            Flags = flags;
        }

        public static ComponentRegistration Create(
            Type contractType,
            Type implementationType,
            ComponentLifetime lifetime,
            ComponentRegistrationFlags flags = ComponentRegistrationFlags.None)
        {
            Ensure.Arg.NotNull(contractType, nameof(contractType));
            Ensure.Arg.NotNull(implementationType, nameof(implementationType));
            Ensure.Arg.OfType(implementationType, contractType, nameof(implementationType));

            return new ComponentRegistration(contractType, implementationType, lifetime, flags);
        }

        public static ComponentRegistration Create<T>(
            Type contractType,
            Func<IContainer, T> implementationFactory,
            ComponentLifetime lifetime,
            ComponentRegistrationFlags flags = ComponentRegistrationFlags.None)
        {
            Ensure.Arg.NotNull(contractType, nameof(contractType));
            Ensure.Arg.NotNull(implementationFactory, nameof(implementationFactory));
            Ensure.Arg.OfType(typeof(T), contractType, nameof(implementationFactory));

            return new ComponentRegistration(contractType, typeof(T), c => implementationFactory(c), lifetime, flags);
        }

        public static ComponentRegistration Create<T>(
            Type contractType,
            T implementationInstance,
            ComponentRegistrationFlags flags = ComponentRegistrationFlags.None)
        {
            Ensure.Arg.NotNull(contractType, nameof(contractType));
            Ensure.Arg.NotNull(implementationInstance, nameof(implementationInstance));
            Ensure.Arg.OfType(typeof(T), contractType, nameof(implementationInstance));

            return new ComponentRegistration(contractType, implementationInstance, flags);
        }
    }
}

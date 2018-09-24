// Licensed under the MIT License.
// Copyright (c) 2018 the AppCore .NET project.

using System;
using AppCore.Diagnostics;

namespace AppCore.DependencyInjection
{
    /// <summary>
    /// Represents parameters for registering a component.
    /// </summary>
    /// <seealso cref="IComponentRegistry"/>
    public readonly struct ComponentRegistration
    {
        /// <summary>
        /// Gets the contract of the component which is registered.
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

        /// <summary>
        /// Creates a <see cref="ComponentRegistration"/> for the specified contract and implementation type.
        /// </summary>
        /// <param name="contractType">The type of the contract.</param>
        /// <param name="implementationType">The type of the implementation.</param>
        /// <param name="lifetime">The component lifetime.</param>
        /// <param name="flags">The registration flags.</param>
        /// <returns>The <see cref="ComponentRegistration"/>.</returns>
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

        /// <summary>
        /// Creates a <see cref="ComponentRegistration"/> for the specified contract with factory delegate.
        /// </summary>
        /// <typeparam name="T">The type of the implementation.</typeparam>
        /// <param name="contractType">The contract type.</param>
        /// <param name="implementationFactory">The factory.</param>
        /// <param name="lifetime">The component lifetime.</param>
        /// <param name="flags">The registration flags.</param>
        /// <returns>The <see cref="ComponentRegistration"/>.</returns>
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

        /// <summary>
        /// Creates a singleton <see cref="ComponentRegistration"/> for the specified contract.
        /// </summary>
        /// <typeparam name="T">The type of the implementation.</typeparam>
        /// <param name="contractType">The type of the contract.</param>
        /// <param name="implementationInstance">The singleton instance.</param>
        /// <param name="flags">The registration flags.</param>
        /// <returns>The <see cref="ComponentRegistration"/>.</returns>
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

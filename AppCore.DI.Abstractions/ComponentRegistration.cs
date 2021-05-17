// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using System;
using AppCore.Diagnostics;

namespace AppCore.DependencyInjection
{
    /// <summary>
    /// Represents parameters for registering a component.
    /// </summary>
    /// <seealso cref="IComponentRegistry"/>
    public class ComponentRegistration
    {
        /// <summary>
        /// Gets the contract of the component which is registered.
        /// </summary>
        public Type ContractType { get; }

        /// <summary>
        /// Gets the <see cref="Type"/> which is instantiated.
        /// </summary>
        public Type ImplementationType { get; }

        /// <summary>
        /// Gets the factory method used to instantiate the component.
        /// </summary>
        public IComponentFactory<object> ImplementationFactory { get; }

        /// <summary>
        /// Gets the singleton instance of the component.
        /// </summary>
        public object ImplementationInstance { get; }

        /// <summary>
        /// Gets the lifetime of the component.
        /// </summary>
        public ComponentLifetime Lifetime { get; }

        private ComponentRegistration(
            Type contractType,
            Type implementationType,
            ComponentLifetime lifetime)
        {
            Ensure.Arg.NotNull(contractType, nameof(contractType));
            Ensure.Arg.NotNull(implementationType, nameof(implementationType));
            Ensure.Arg.OfType(implementationType, contractType, nameof(implementationType));

            ContractType = contractType;
            ImplementationType = implementationType;
            Lifetime = lifetime;
        }

        private ComponentRegistration(
            Type contractType,
            IComponentFactory<object> implementationFactory,
            ComponentLifetime lifetime)
        {
            Ensure.Arg.NotNull(contractType, nameof(contractType));
            Ensure.Arg.NotNull(implementationFactory, nameof(implementationFactory));

            ContractType = contractType;
            ImplementationFactory = implementationFactory;
            Lifetime = lifetime;
        }

        private ComponentRegistration(
            Type contractType,
            object implementationInstance)
        {
            Ensure.Arg.NotNull(contractType, nameof(contractType));
            Ensure.Arg.NotNull(implementationInstance, nameof(implementationInstance));
            Ensure.Arg.OfType(implementationInstance.GetType(), contractType, nameof(implementationInstance));

            ContractType = contractType;
            ImplementationInstance = implementationInstance;
            Lifetime = ComponentLifetime.Singleton;
        }

        /// <summary>
        /// Gets the implementation type of the component.
        /// </summary>
        /// <returns>The implementation type.</returns>
        public Type GetImplementationType()
        {
            if (ImplementationType != null)
                return ImplementationType;

            if (ImplementationInstance != null)
                return ImplementationInstance.GetType();

            if (ImplementationFactory != null)
                return ImplementationFactory.GetType().GenericTypeArguments[0];

            throw new InvalidOperationException("Internal error");
        }

        /// <summary>
        /// Creates a singleton <see cref="ComponentRegistration"/> for the specified contract.
        /// </summary>
        /// <param name="contractType">The type of the contract.</param>
        /// <param name="instance">The singleton instance.</param>
        /// <returns>The <see cref="ComponentRegistration"/>.</returns>
        public static ComponentRegistration Create(Type contractType, object instance)
        {
            return new ComponentRegistration(contractType, instance);
        }

        /// <summary>
        /// Creates a singleton <see cref="ComponentRegistration"/> for the specified contract.
        /// </summary>
        /// <typeparam name="TContract">The type of the contract.</typeparam>
        /// <param name="instance">The singleton instance.</param>
        /// <returns>The <see cref="ComponentRegistration"/>.</returns>
        public static ComponentRegistration Create<TContract>(TContract instance)
            where TContract : class
        {
            return new ComponentRegistration(typeof(TContract), instance);
        }

        /// <summary>
        /// Creates a <see cref="ComponentRegistration"/> for the specified contract and implementation type.
        /// </summary>
        /// <param name="contractType">The type of the contract.</param>
        /// <param name="implementationType">The type of the implementation.</param>
        /// <param name="lifetime">The component lifetime.</param>
        /// <returns>The <see cref="ComponentRegistration"/>.</returns>
        public static ComponentRegistration Create(
            Type contractType,
            Type implementationType,
            ComponentLifetime lifetime)
        {
            return new ComponentRegistration(contractType, implementationType, lifetime);
        }

        /// <summary>
        /// Creates a <see cref="ComponentRegistration"/> for the specified contract and implementation type.
        /// </summary>
        /// <typeparam name="TContract">The type of the contract.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        /// <param name="lifetime">The component lifetime.</param>
        /// <returns>The <see cref="ComponentRegistration"/>.</returns>
        public static ComponentRegistration Create<TContract, TImplementation>(ComponentLifetime lifetime)
            where TContract : class
            where TImplementation : class, TContract
        {
            return new ComponentRegistration(typeof(TContract), typeof(TImplementation), lifetime);
        }

        /// <summary>
        /// Creates a <see cref="ComponentRegistration"/> for the specified contract with factory delegate.
        /// </summary>
        /// <param name="contractType">The contract type.</param>
        /// <param name="factory">The factory.</param>
        /// <param name="lifetime">The component lifetime.</param>
        /// <returns>The <see cref="ComponentRegistration"/>.</returns>
        public static ComponentRegistration Create(
            Type contractType,
            IComponentFactory<object> factory,
            ComponentLifetime lifetime)
        {
            return new ComponentRegistration(contractType, factory, lifetime);
        }

        /// <summary>
        /// Creates a <see cref="ComponentRegistration"/> for the specified contract with factory delegate.
        /// </summary>
        /// <typeparam name="TContract">The contract type.</typeparam>
        /// <param name="factory">The factory.</param>
        /// <param name="lifetime">The component lifetime.</param>
        /// <returns>The <see cref="ComponentRegistration"/>.</returns>
        public static ComponentRegistration Create<TContract>(
            IComponentFactory<TContract> factory,
            ComponentLifetime lifetime)
            where TContract : class
        {
            return new ComponentRegistration(typeof(TContract), factory, lifetime);
        }

        /// <summary>
        /// Creates a singleton <see cref="ComponentRegistration"/> for the specified contract.
        /// </summary>
        /// <param name="contractType">The type of the contract.</param>
        /// <param name="instance">The singleton instance.</param>
        /// <returns>The <see cref="ComponentRegistration"/>.</returns>
        public static ComponentRegistration Singleton(Type contractType, object instance)
        {
            return Create(contractType, instance);
        }

        /// <summary>
        /// Creates a singleton <see cref="ComponentRegistration"/> for the specified contract.
        /// </summary>
        /// <typeparam name="TContract">The type of the contract.</typeparam>
        /// <param name="instance">The singleton instance.</param>
        /// <returns>The <see cref="ComponentRegistration"/>.</returns>
        public static ComponentRegistration Singleton<TContract>(TContract instance)
            where TContract : class
        {
            return Create(instance);
        }

        public static ComponentRegistration Singleton(Type contractType, Type implementationType)
        {
            return Create(contractType, implementationType, ComponentLifetime.Singleton);
        }

        public static ComponentRegistration Singleton<TContract, TImplementation>()
            where TContract : class
            where TImplementation : class, TContract
        {
            return Create<TContract, TImplementation>(ComponentLifetime.Singleton);
        }

        public static ComponentRegistration Singleton(Type contractType, IComponentFactory<object> factory)
        {
            return Create(contractType, factory, ComponentLifetime.Singleton);
        }

        public static ComponentRegistration Singleton<TContract>(IComponentFactory<TContract> factory)
            where TContract : class
        {
            return Create(factory, ComponentLifetime.Singleton);
        }

        public static ComponentRegistration Transient(Type contractType, Type implementationType)
        {
            return Create(contractType, implementationType, ComponentLifetime.Transient);
        }

        public static ComponentRegistration Transient<TContract, TImplementation>()
            where TContract : class
            where TImplementation : class, TContract
        {
            return Create<TContract, TImplementation>(ComponentLifetime.Transient);
        }

        public static ComponentRegistration Transient(Type contractType, IComponentFactory<object> factory)
        {
            return Create(contractType, factory, ComponentLifetime.Transient);
        }

        public static ComponentRegistration Transient<TContract>(IComponentFactory<TContract> factory)
            where TContract : class
        {
            return Create(factory, ComponentLifetime.Transient);
        }

        public static ComponentRegistration Scoped(Type contractType, Type implementationType)
        {
            return Create(contractType, implementationType, ComponentLifetime.Scoped);
        }
        
        public static ComponentRegistration Scoped<TContract, TImplementation>()
            where TContract : class
            where TImplementation : class, TContract
        {
            return Create<TContract, TImplementation>(ComponentLifetime.Scoped);
        }

        public static ComponentRegistration Scoped(Type contractType, IComponentFactory<object> factory)
        {
            return Create(contractType, factory, ComponentLifetime.Scoped);
        }

        public static ComponentRegistration Scoped<TContract>(IComponentFactory<TContract> factory)
            where TContract : class
        {
            return Create(factory, ComponentLifetime.Scoped);
        }
    }
}

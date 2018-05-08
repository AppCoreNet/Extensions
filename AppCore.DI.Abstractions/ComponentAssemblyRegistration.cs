using System;
using System.Collections.Generic;
using System.Reflection;
using AppCore.Diagnostics;

namespace AppCore.DependencyInjection
{
    /// <summary>
    /// Represents parameters for registration of components from a set of assemblies.
    /// </summary>
    /// <seealso cref="IComponentRegistry"/>
    public readonly struct ComponentAssemblyRegistration
    {
        /// <summary>
        /// Gets the set of assemblies which are scanned for services.
        /// </summary>
        public IEnumerable<Assembly> Assemblies { get; }

        /// <summary>
        /// Gets the contract type of components to register.
        /// </summary>
        public Type ContractType { get; }

        /// <summary>
        /// Gets the default <see cref="ComponentLifetime"/> for registered components.
        /// </summary>
        /// <seealso cref="LifetimeAttribute"/>
        public ComponentLifetime DefaultLifetime { get; }

        /// <summary>
        /// Gets the flags used when registering components.
        /// </summary>
        public ComponentRegistrationFlags Flags { get; }

        private ComponentAssemblyRegistration(
            IEnumerable<Assembly> assemblies,
            Type contractType,
            ComponentLifetime defaultLifetime,
            ComponentRegistrationFlags flags)
        {
            Ensure.Arg.NotNull(assemblies, nameof(assemblies));
            Ensure.Arg.NotNull(contractType, nameof(contractType));

            Assemblies = assemblies;
            ContractType = contractType;
            DefaultLifetime = defaultLifetime;
            Flags = flags;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="ComponentAssemblyRegistration"/> type.
        /// </summary>
        /// <param name="assemblies"></param>
        /// <param name="contractType">The contract type of components to register.</param>
        /// <param name="defaultLifetime">Default lifetime of components.</param>
        /// <param name="flags">Component registration flags.</param>
        /// <returns>A new instance of <see cref="ComponentAssemblyRegistration"/>.</returns>
        public static ComponentAssemblyRegistration Create(
            IEnumerable<Assembly> assemblies,
            Type contractType,
            ComponentLifetime defaultLifetime,
            ComponentRegistrationFlags flags = ComponentRegistrationFlags.None)
        {
            return new ComponentAssemblyRegistration(assemblies, contractType, defaultLifetime, flags);
        }
    }
}
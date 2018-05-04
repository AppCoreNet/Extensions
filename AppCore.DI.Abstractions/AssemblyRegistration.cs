using System;
using System.Collections.Generic;
using System.Reflection;
using AppCore.Diagnostics;

namespace AppCore.DependencyInjection
{
    /// <summary>
    /// Represents parameters for registration of services from a set of assemblies.
    /// </summary>
    /// <seealso cref="IServiceRegistrar"/>
    public readonly struct AssemblyRegistration
    {
        /// <summary>
        /// Gets the set of assemblies which are scanned for services.
        /// </summary>
        public IEnumerable<Assembly> Assemblies { get; }

        /// <summary>
        /// Gets the type of services to register.
        /// </summary>
        public Type ServiceType { get; }

        /// <summary>
        /// Gets the default <see cref="ServiceLifetime"/> of registered services.
        /// </summary>
        /// <seealso cref="ServiceLifetimeAttribute"/>
        public ServiceLifetime DefaultLifetime { get; }

        /// <summary>
        /// Gets the flags used when registering services.
        /// </summary>
        public ServiceRegistrationFlags Flags { get; }

        private AssemblyRegistration(
            IEnumerable<Assembly> assemblies,
            Type serviceType,
            ServiceLifetime defaultLifetime,
            ServiceRegistrationFlags flags)
        {
            Ensure.Arg.NotNull(assemblies, nameof(assemblies));
            Ensure.Arg.NotNull(serviceType, nameof(serviceType));

            Assemblies = assemblies;
            ServiceType = serviceType;
            DefaultLifetime = defaultLifetime;
            Flags = flags;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="AssemblyRegistration"/> type.
        /// </summary>
        /// <param name="assemblies"></param>
        /// <param name="serviceType">The type of service to register.</param>
        /// <param name="defaultLifetime">Default lifetime of services.</param>
        /// <param name="flags">Service registration flags.</param>
        /// <returns>A new instance of <see cref="AssemblyRegistration"/>.</returns>
        public static AssemblyRegistration Create(
            IEnumerable<Assembly> assemblies,
            Type serviceType,
            ServiceLifetime defaultLifetime,
            ServiceRegistrationFlags flags = ServiceRegistrationFlags.None)
        {
            return new AssemblyRegistration(assemblies, serviceType, defaultLifetime, flags);
        }
    }
}
using System;

namespace AppCore.DependencyInjection
{
    /// <summary>
    /// Specifies the lifetime of a service when registered via assembly scanning.
    /// </summary>
    /// <seealso cref="ServiceLifetime"/>
    [AttributeUsage(AttributeTargets.Class)]
    public class ServiceLifetimeAttribute : Attribute
    {
        /// <summary>
        /// Gets the <see cref="ServiceLifetime"/>.
        /// </summary>
        public ServiceLifetime Lifetime { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceLifetimeAttribute"/> class.
        /// </summary>
        /// <param name="lifetime">The lifetime of the service.</param>
        public ServiceLifetimeAttribute(ServiceLifetime lifetime)
        {
            Lifetime = lifetime;
        }
    }
}

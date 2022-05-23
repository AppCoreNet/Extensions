// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using System;
using Microsoft.Extensions.DependencyInjection;

namespace AppCore.DependencyInjection
{
    /// <summary>
    /// Specifies the lifetime of a service when registered via dynamic scanning.
    /// </summary>
    /// <seealso cref="ServiceLifetime"/>
    [AttributeUsage(AttributeTargets.Class)]
    public class LifetimeAttribute : Attribute
    {
        /// <summary>
        /// Gets the <see cref="ServiceLifetime"/>.
        /// </summary>
        public ServiceLifetime Lifetime { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LifetimeAttribute"/> class.
        /// </summary>
        /// <param name="lifetime">The lifetime of the service.</param>
        public LifetimeAttribute(ServiceLifetime lifetime)
        {
            Lifetime = lifetime;
        }
    }
}

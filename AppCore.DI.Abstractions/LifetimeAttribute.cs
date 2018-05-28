// Licensed under the MIT License.
// Copyright (c) 2018 the AppCore .NET project.

using System;

namespace AppCore.DependencyInjection
{
    /// <summary>
    /// Specifies the lifetime of a component when registered via assembly scanning.
    /// </summary>
    /// <seealso cref="ComponentLifetime"/>
    [AttributeUsage(AttributeTargets.Class)]
    public class LifetimeAttribute : Attribute
    {
        /// <summary>
        /// Gets the <see cref="ComponentLifetime"/>.
        /// </summary>
        public ComponentLifetime Lifetime { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LifetimeAttribute"/> class.
        /// </summary>
        /// <param name="lifetime">The lifetime of the component.</param>
        public LifetimeAttribute(ComponentLifetime lifetime)
        {
            Lifetime = lifetime;
        }
    }
}

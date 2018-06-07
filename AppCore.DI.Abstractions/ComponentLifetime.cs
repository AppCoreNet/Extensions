// Licensed under the MIT License.
// Copyright (c) 2018 the AppCore .NET project.

namespace AppCore.DependencyInjection
{
    /// <summary>
    /// Represents the lifetime of a component.
    /// </summary>
    /// <seealso cref="ComponentRegistration"/>
    public enum ComponentLifetime
    {
        /// <summary>
        /// The component is instantiated each time it is resolved.
        /// </summary>
        Transient,

        /// <summary>
        /// The component is instantiated only once.
        /// </summary>
        Singleton,

        /// <summary>
        /// The component is instantiated per scope.
        /// </summary>
        Scoped,
    }
}
// Licensed under the MIT License.
// Copyright (c) 2018 the AppCore .NET project.

using System;

namespace AppCore.DependencyInjection
{
    /// <summary>
    /// Represents the dependency injection container.
    /// </summary>
    /// <seealso cref="IComponentRegistry"/>
    /// <seealso cref="ContainerExtensions"/>
    public interface IContainer
    {
        /// <summary>
        /// Gets the capabilities of the container.
        /// </summary>
        ContainerCapabilities Capabilities { get; }

        /// <summary>
        /// Resolves a component with the given contract.
        /// </summary>
        /// <param name="contractType">The contract of the component.</param>
        /// <returns>The resolved component.</returns>
        object Resolve(Type contractType);

        /// <summary>
        /// Resolves an optional component with the given contract.
        /// </summary>
        /// <param name="contractType">The contract of the component.</param>
        /// <returns>The resolved component.</returns>
        object ResolveOptional(Type contractType);
    }
}

// Licensed under the MIT License.
// Copyright (c) 2018 the AppCore .NET project.

namespace AppCore.DependencyInjection
{
    /// <summary>
    /// Represents a factory to create a <see cref="IContainerScope"/>.
    /// </summary>
    public interface IContainerScopeFactory
    {
        /// <summary>
        /// Creates a <see cref="IContainerScope"/>.
        /// </summary>
        /// <returns>The <see cref="IContainerScope"/>.</returns>
        IContainerScope CreateScope();
    }
}
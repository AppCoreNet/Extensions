// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

namespace AppCore.DependencyInjection
{
    /// <summary>
    /// Represents a factory for components.
    /// </summary>
    /// <typeparam name="T">The type of the component.</typeparam>
    public interface IComponentFactory<out T>
        where T : class
    {
        /// <summary>
        /// Creates an instance of the component.
        /// </summary>
        /// <param name="container">The DI container.</param>
        /// <returns>The component instance.</returns>
        T Create(IContainer container);
    }
}
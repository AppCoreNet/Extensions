// Licensed under the MIT License.
// Copyright (c) 2018 the AppCore .NET project.

using AppCore.Diagnostics;

namespace AppCore.DependencyInjection.StructureMap
{
    /// <summary>
    /// Represents StructureMap <see cref="IContainerScopeFactory"/>.
    /// </summary>
    public class StructureMapContainerScopeFactory : IContainerScopeFactory
    {
        private readonly global::StructureMap.IContainer _container;

        /// <summary>
        /// Initializes a new instance of the <see cref="StructureMapContainerScopeFactory"/>.
        /// </summary>
        /// <param name="container">The <see cref="StructureMap.IContainer"/>.</param>
        public StructureMapContainerScopeFactory(global::StructureMap.IContainer container)
        {
            Ensure.Arg.NotNull(container, nameof(container));
            _container = container;
        }

        /// <inheritdoc />
        public IContainerScope CreateScope()
        {
            return new StructureMapContainerScope(_container.GetNestedContainer());
        }
    }
}
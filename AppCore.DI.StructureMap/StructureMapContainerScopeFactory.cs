// Licensed under the MIT License.
// Copyright (c) 2018 the AppCore .NET project.

using AppCore.Diagnostics;
using SM = StructureMap;

namespace AppCore.DependencyInjection.StructureMap
{
    /// <summary>
    /// Represents StructureMap <see cref="IContainerScopeFactory"/>.
    /// </summary>
    public class StructureMapContainerScopeFactory : IContainerScopeFactory
    {
        private readonly SM.IContainer _container;

        /// <summary>
        /// Initializes a new instance of the <see cref="StructureMapContainerScopeFactory"/>.
        /// </summary>
        /// <param name="container">The <see cref="SM.IContainer"/>.</param>
        public StructureMapContainerScopeFactory(SM.IContainer container)
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
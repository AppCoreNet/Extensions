// Licensed under the MIT License.
// Copyright (c) 2018 the AppCore .NET project.

using System;
using AppCore.Diagnostics;
using SM = StructureMap;

namespace AppCore.DependencyInjection.StructureMap
{
    /// <summary>
    /// Represents StructureMap <see cref="IContainerScope"/>.
    /// </summary>
    public class StructureMapContainerScope : IContainerScope
    {
        private readonly SM.IContainer _internalContainer;
        private readonly IContainer _container;

        /// <inheritdoc />
        public IContainer Container => _container;

        /// <summary>
        /// Initializes a new instance of the <see cref="StructureMapContainerScope"/>.
        /// </summary>
        /// <param name="container">The <see cref="SM.IContainer"/>.</param>
        public StructureMapContainerScope(SM.IContainer container)
        {
            Ensure.Arg.NotNull(container, nameof(container));
            _internalContainer = container;
            _container = container.GetInstance<IContainer>();
        }

        /// <summary>
        /// Allows an object to try to free resources and perform other cleanup operations before
        /// it is reclaimed by garbage collection.
        /// </summary>
        ~StructureMapContainerScope()
        {
            Dispose(false);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                _internalContainer.Dispose();
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged
        /// resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
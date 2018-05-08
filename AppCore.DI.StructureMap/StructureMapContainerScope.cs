using System;
using AppCore.Diagnostics;

namespace AppCore.DependencyInjection.StructureMap
{
    public class StructureMapContainerScope : IContainerScope
    {
        private readonly global::StructureMap.IContainer _internalContainer;
        private readonly IContainer _container;

        public IContainer Container => _container;

        public StructureMapContainerScope(global::StructureMap.IContainer container)
        {
            Ensure.Arg.NotNull(container, nameof(container));
            _internalContainer = container;
            _container = container.GetInstance<IContainer>();
        }

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

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
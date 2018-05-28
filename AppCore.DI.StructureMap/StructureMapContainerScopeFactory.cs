// Licensed under the MIT License.
// Copyright (c) 2018 the AppCore .NET project.

using AppCore.Diagnostics;

namespace AppCore.DependencyInjection.StructureMap
{
    public class StructureMapContainerScopeFactory : IContainerScopeFactory
    {
        private readonly global::StructureMap.IContainer _container;

        public StructureMapContainerScopeFactory(global::StructureMap.IContainer container)
        {
            Ensure.Arg.NotNull(container, nameof(container));
            _container = container;
        }

        public IContainerScope CreateScope()
        {
            return new StructureMapContainerScope(_container.GetNestedContainer());
        }
    }
}
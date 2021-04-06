// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using StructureMap;

namespace AppCore.DependencyInjection.StructureMap
{
    public class StructureMapContainerScopeTests : ContainerScopeTests
    {
        public override IComponentRegistry Registry { get; }

        private Registry SMRegistry { get; }

        public StructureMapContainerScopeTests()
        {
            SMRegistry = new Registry();
            Registry = new StructureMapComponentRegistry(SMRegistry);
        }

        public override IContainerScope CreateScope()
        {
            var container = new StructureMapContainer(new Container(SMRegistry));

            return new StructureMapContainerScope(
                container.Resolve<global::StructureMap.IContainer>()
                         .GetNestedContainer());
        }
    }
}

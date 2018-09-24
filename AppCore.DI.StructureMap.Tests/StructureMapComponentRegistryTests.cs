// Licensed under the MIT License.
// Copyright (c) 2018 the AppCore .NET project.

using StructureMap;

namespace AppCore.DependencyInjection
{
    public class StructureMapComponentRegistryTests : ComponentRegistryTests
    {
        public override IComponentRegistry Registry { get; }

        public StructureMapComponentRegistryTests()
        {
            var registry = new StructureMapComponentRegistry();
            Registry = registry;
        }

        protected override IContainer BuildContainer()
        {
            return new StructureMapContainer(
                new Container(c => ((StructureMapComponentRegistry) Registry).RegisterComponents(c)));
        }
    }
}
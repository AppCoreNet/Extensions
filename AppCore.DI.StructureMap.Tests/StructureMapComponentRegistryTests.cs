// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using StructureMap;

namespace AppCore.DependencyInjection.StructureMap
{
    public class StructureMapComponentRegistryTests : ComponentRegistryTests
    {
        public override IComponentRegistry Registry { get; }

        private Registry SMRegistry { get; }

        public StructureMapComponentRegistryTests()
        {
            SMRegistry = new Registry();
            Registry = new StructureMapComponentRegistry(SMRegistry);
        }

        protected override IContainer BuildContainer()
        {
            return new StructureMapContainer(new Container(SMRegistry));
        }
    }
}
using StructureMap;

namespace AppCore.DependencyInjection.StructureMap
{
    public class StructureMapContainerScopeTests : ContainerScopeTests
    {
        public override IComponentRegistry Registry { get; }

        public StructureMapContainerScopeTests()
        {
            Registry = new StructureMapComponentRegistry();
        }

        public override IContainerScope CreateScope()
        {
            var container = new StructureMapContainer(
                new Container(c => c.AddRegistry((StructureMapComponentRegistry) Registry)));

            return new StructureMapContainerScope(
                container.Resolve<global::StructureMap.IContainer>()
                         .GetNestedContainer());
        }
    }
}

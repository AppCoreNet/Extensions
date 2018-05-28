// Licensed under the MIT License.
// Copyright (c) 2018 the AppCore .NET project.

using Autofac;

namespace AppCore.DependencyInjection.Autofac
{
    public class AutofacComponentRegistryTests : ComponentRegistryTests
    {
        public ContainerBuilder ContainerBuilder { get; }

        public override IComponentRegistry Registry { get; }

        public AutofacComponentRegistryTests()
        {
            ContainerBuilder = new ContainerBuilder();
            Registry = new AutofacComponentRegistry();
        }

        protected override IContainer BuildContainer()
        {
            ((AutofacComponentRegistry)Registry).RegisterComponents(ContainerBuilder);
            return new AutofacContainer(ContainerBuilder.Build());
        }
    }
}
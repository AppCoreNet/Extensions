// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

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
            Registry = new AutofacComponentRegistry(ContainerBuilder);
        }

        protected override IContainer BuildContainer()
        {
            return new AutofacContainer(ContainerBuilder.Build());
        }
    }
}
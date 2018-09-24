// Licensed under the MIT License.
// Copyright (c) 2018 the AppCore .NET project.

using Autofac;

namespace AppCore.DependencyInjection
{
    public class AutofacContainerScopeTests : ContainerScopeTests
    {
        public override IComponentRegistry Registry { get; }

        public ContainerBuilder Builder { get; }

        public AutofacContainerScopeTests()
        {
            Builder = new ContainerBuilder();
            Registry = new AutofacComponentRegistry();
        }

        public override IContainerScope CreateScope()
        {
            ((AutofacComponentRegistry) Registry).RegisterComponents(Builder);
            return new AutofacContainerScope(Builder.Build().BeginLifetimeScope());
        }
    }
}

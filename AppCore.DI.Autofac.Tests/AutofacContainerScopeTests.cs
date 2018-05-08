using System;
using System.Collections.Generic;
using System.Text;
using Autofac;

namespace AppCore.DependencyInjection.Autofac
{
    public class AutofacContainerScopeTests : ContainerScopeTests
    {
        public override IComponentRegistry Registry { get; }

        public ContainerBuilder Builder { get; }

        public AutofacContainerScopeTests()
        {
            Builder = new ContainerBuilder();
            Registry = new AutofacComponentRegistry(Builder);
        }

        public override IContainerScope CreateScope()
        {
            return new AutofacContainerScope(Builder.Build().BeginLifetimeScope());
        }
    }
}

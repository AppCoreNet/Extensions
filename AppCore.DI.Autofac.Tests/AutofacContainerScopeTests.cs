// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using Autofac;

namespace AppCore.DependencyInjection.Autofac
{
    public class AutofacContainerScopeTests : ContainerScopeTests
    {
        public override IComponentRegistry Registry { get; }

        public ContainerBuilder Builder { get; }

        private global::Autofac.IContainer _container;

        public AutofacContainerScopeTests()
        {
            Builder = new ContainerBuilder();
            Registry = new AutofacComponentRegistry(Builder);
        }

        public override IContainerScope CreateScope()
        {
            _container ??= Builder.Build();
            return new AutofacContainerScope(_container.BeginLifetimeScope());
        }
    }
}

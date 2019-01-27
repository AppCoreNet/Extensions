// Licensed under the MIT License.
// Copyright (c) 2018 the AppCore .NET project.

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
            Registry = new AutofacComponentRegistry();
        }

        public override IContainerScope CreateScope()
        {
            if (_container == null)
            {
                ((AutofacComponentRegistry) Registry).RegisterComponents(Builder);
                _container = Builder.Build();
            }

            return new AutofacContainerScope(_container.BeginLifetimeScope());
        }
    }
}

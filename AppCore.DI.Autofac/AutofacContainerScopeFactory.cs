// Licensed under the MIT License.
// Copyright (c) 2018 the AppCore .NET project.

using AppCore.Diagnostics;
using Autofac;

namespace AppCore.DependencyInjection.Autofac
{
    public class AutofacContainerScopeFactory : IContainerScopeFactory
    {
        private readonly ILifetimeScope _lifetimeScope;

        public AutofacContainerScopeFactory(ILifetimeScope lifetimeScope)
        {
            Ensure.Arg.NotNull(lifetimeScope, nameof(lifetimeScope));
            _lifetimeScope = lifetimeScope;
        }

        public IContainerScope CreateScope()
        {
            return new AutofacContainerScope(_lifetimeScope.BeginLifetimeScope());
        }
    }
}
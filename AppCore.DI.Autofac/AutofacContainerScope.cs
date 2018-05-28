// Licensed under the MIT License.
// Copyright (c) 2018 the AppCore .NET project.

using System;
using AppCore.Diagnostics;
using Autofac;

namespace AppCore.DependencyInjection.Autofac
{
    public class AutofacContainerScope : IContainerScope
    {
        private readonly ILifetimeScope _lifetimeScope;
        private readonly IContainer _container;

        public IContainer Container => _container;

        public AutofacContainerScope(ILifetimeScope lifetimeScope)
        {
            Ensure.Arg.NotNull(lifetimeScope, nameof(lifetimeScope));

            _lifetimeScope = lifetimeScope;
            _container = lifetimeScope.Resolve<IContainer>();
        }

        ~AutofacContainerScope()
        {
            Dispose(false);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                _lifetimeScope.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
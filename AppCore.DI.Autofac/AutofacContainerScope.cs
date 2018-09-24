// Licensed under the MIT License.
// Copyright (c) 2018 the AppCore .NET project.

using System;
using AppCore.Diagnostics;
using Autofac;

namespace AppCore.DependencyInjection
{
    /// <summary>
    /// Represents a Autofac container scope.
    /// </summary>
    public class AutofacContainerScope : IContainerScope
    {
        private readonly ILifetimeScope _lifetimeScope;

        /// <inheritdoc />
        public IContainer Container { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AutofacContainerScope"/> class.
        /// </summary>
        /// <param name="lifetimeScope">The lifetime scope.</param>
        public AutofacContainerScope(ILifetimeScope lifetimeScope)
        {
            Ensure.Arg.NotNull(lifetimeScope, nameof(lifetimeScope));

            _lifetimeScope = lifetimeScope;
            Container = lifetimeScope.Resolve<IContainer>();
        }

        /// <summary>
        /// Allows an object to try to free resources and perform other cleanup operations before
        /// it is reclaimed by garbage collection.
        /// </summary>
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

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing,
        /// or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
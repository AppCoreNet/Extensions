// Licensed under the MIT License.
// Copyright (c) 2018 the AppCore .NET project.

using AppCore.Diagnostics;
using Autofac;

namespace AppCore.DependencyInjection
{
    /// <summary>
    /// Represents Autofac based <see cref="IContainerScopeFactory"/>.
    /// </summary>
    public class AutofacContainerScopeFactory : IContainerScopeFactory
    {
        private readonly ILifetimeScope _lifetimeScope;

        /// <summary>
        /// Initializes a new instance of the <see cref="AutofacContainerScopeFactory"/> class.
        /// </summary>
        /// <param name="lifetimeScope">The lifetime scope.</param>
        public AutofacContainerScopeFactory(ILifetimeScope lifetimeScope)
        {
            Ensure.Arg.NotNull(lifetimeScope, nameof(lifetimeScope));
            _lifetimeScope = lifetimeScope;
        }

        /// <inheritdoc />
        public IContainerScope CreateScope()
        {
            return new AutofacContainerScope(_lifetimeScope.BeginLifetimeScope());
        }
    }
}
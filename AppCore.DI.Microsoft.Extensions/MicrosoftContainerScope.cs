// Licensed under the MIT License.
// Copyright (c) 2018 the AppCore .NET project.

using System;
using AppCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace AppCore.DependencyInjection
{
    /// <summary>
    /// Represents Microsoft DI based <see cref="IContainerScope"/> implementation.
    /// </summary>
    public class MicrosoftContainerScope : IContainerScope
    {
        private readonly IServiceScope _serviceScope;
        private readonly MicrosoftContainer _container;

        /// <inheritdoc />
        public IContainer Container => _container;

        /// <summary>
        /// Initializes a new instance of the <see cref="MicrosoftContainerScope"/> class.
        /// </summary>
        /// <param name="serviceScope">The <see cref="IServiceScope"/>.</param>
        public MicrosoftContainerScope(IServiceScope serviceScope)
        {
            Ensure.Arg.NotNull(serviceScope, nameof(serviceScope));

            _serviceScope = serviceScope;
            _container = new MicrosoftContainer(serviceScope.ServiceProvider);
        }

        /// <summary>
        /// Allows an object to try to free resources and perform other cleanup operations before
        /// it is reclaimed by garbage collection.
        /// </summary>
        ~MicrosoftContainerScope()
        {
            Dispose(false);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                _serviceScope.Dispose();
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged
        /// resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
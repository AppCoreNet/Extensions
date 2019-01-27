// Licensed under the MIT License.
// Copyright (c) 2018 the AppCore .NET project.

using AppCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace AppCore.DependencyInjection.Microsoft.Extensions
{
    /// <summary>
    /// Represents a Microsoft DI based <see cref="IContainerScopeFactory"/>.
    /// </summary>
    public class MicrosoftContainerScopeFactory : IContainerScopeFactory
    {
        private readonly IServiceScopeFactory _factory;

        /// <summary>
        /// Initializes a new instance of the <see cref="MicrosoftContainerScopeFactory"/>.
        /// </summary>
        /// <param name="factory">The <see cref="IServiceScopeFactory"/>.</param>
        public MicrosoftContainerScopeFactory(IServiceScopeFactory factory)
        {
            Ensure.Arg.NotNull(factory, nameof(factory));
            _factory = factory;
        }

        /// <inheritdoc />
        public IContainerScope CreateScope()
        {
            return new MicrosoftContainerScope(_factory.CreateScope());
        }
    }
}
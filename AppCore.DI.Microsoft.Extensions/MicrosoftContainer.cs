// Licensed under the MIT License.
// Copyright (c) 2018 the AppCore .NET project.

using System;
using AppCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace AppCore.DependencyInjection.Microsoft.Extensions
{
    /// <summary>
    /// Represents Microsoft DI based <see cref="IContainer"/> implementation.
    /// </summary>
    public class MicrosoftContainer : IContainer
    {
        private readonly IServiceProvider _serviceProvider;

        /// <inheritdoc />
        public ContainerCapabilities Capabilities { get; } = ContainerCapabilities.None;

        /// <summary>
        /// Initializes a new instance of the <see cref="MicrosoftContainer"/> class.
        /// </summary>
        /// <param name="serviceProvider">The <see cref="IServiceProvider"/>.</param>
        public MicrosoftContainer(IServiceProvider serviceProvider)
        {
            Ensure.Arg.NotNull(serviceProvider, nameof(serviceProvider));
            _serviceProvider = serviceProvider;
        }

        /// <inheritdoc />
        public object Resolve(Type contractType)
        {
            return _serviceProvider.GetRequiredService(contractType);
        }

        /// <inheritdoc />
        public object ResolveOptional(Type contractType)
        {
            return _serviceProvider.GetService(contractType);
        }
    }
}
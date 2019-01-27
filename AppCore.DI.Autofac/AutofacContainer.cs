// Licensed under the MIT License.
// Copyright (c) 2018 the AppCore .NET project.

using System;
using System.Linq;
using AppCore.Diagnostics;
using Autofac;
using Autofac.Features.Variance;

namespace AppCore.DependencyInjection.Autofac
{
    /// <summary>
    /// Autofac based <see cref="IContainer"/> implementation.
    /// </summary>
    public class AutofacContainer : IContainer
    {
        private readonly IComponentContext _context;

        /// <inheritdoc />
        public ContainerCapabilities Capabilities { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AutofacContainer"/> class.
        /// </summary>
        /// <param name="context">The <see cref="IComponentContext"/>.</param>
        public AutofacContainer(IComponentContext context)
        {
            Ensure.Arg.NotNull(context, nameof(context));

            _context = context;

            var capabilities = ContainerCapabilities.None;
            if (context.ComponentRegistry.Sources.OfType<ContravariantRegistrationSource>().Any())
                capabilities = capabilities | ContainerCapabilities.ContraVariance;

            Capabilities = capabilities;
        }

        /// <inheritdoc />
        public object Resolve(Type contractType)
        {
            return _context.Resolve(contractType);
        }

        /// <inheritdoc />
        public object ResolveOptional(Type contractType)
        {
            return _context.ResolveOptional(contractType);
        }
    }
}
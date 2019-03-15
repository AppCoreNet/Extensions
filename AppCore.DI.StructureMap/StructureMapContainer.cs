// Licensed under the MIT License.
// Copyright (c) 2018 the AppCore .NET project.

using System;
using System.Collections.Generic;
using AppCore.Diagnostics;
using SM = StructureMap;

namespace AppCore.DependencyInjection.StructureMap
{
    /// <summary>
    /// StructureMap based <see cref="IContainer"/> implementation.
    /// </summary>
    public class StructureMapContainer : IContainer
    {
        private readonly SM.IContainer _container;

        /// <inheritdoc />
        public ContainerCapabilities Capabilities { get; } = ContainerCapabilities.ContraVariance;

        /// <summary>
        /// Initializes a new instance of the <see cref="StructureMapContainer"/> class.
        /// </summary>
        /// <param name="container">The <see cref="SM.IContainer"/>.</param>
        public StructureMapContainer(SM.IContainer container)
        {
            Ensure.Arg.NotNull(container, nameof(container));
            _container = container;
        }

        /// <inheritdoc />
        public object Resolve(Type contractType)
        {
            return _container.GetInstance(contractType);
        }

        /// <inheritdoc />
        public object ResolveOptional(Type contractType)
        {
            Ensure.Arg.NotNull(contractType, nameof(contractType));

            if (contractType.IsClosedTypeOf(typeof(IEnumerable<>)))
            {
                return _container.GetInstance(contractType);
            }

            return _container.TryGetInstance(contractType);
        }
    }
}
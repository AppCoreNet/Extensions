// Licensed under the MIT License.
// Copyright (c) 2018 the AppCore .NET project.

using System;
using System.Collections.Generic;
using AppCore.Diagnostics;

namespace AppCore.DependencyInjection.StructureMap
{
    /// <summary>
    /// StructureMap based <see cref="IContainer"/> implementation.
    /// </summary>
    public class StructureMapContainer : IContainer
    {
        private readonly global::StructureMap.IContainer _container;

        public ContainerCapabilities Capabilities { get; } = ContainerCapabilities.ContraVariance;

        public StructureMapContainer(global::StructureMap.IContainer container)
        {
            Ensure.Arg.NotNull(container, nameof(container));
            _container = container;
        }

        public object Resolve(Type contractType)
        {
            return _container.GetInstance(contractType);
        }

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
// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using System;
using AppCore.DependencyInjection;
using AppCore.DependencyInjection.Activator;

namespace AppCore.Hosting.Plugins
{
    internal class StartupContainer : IContainer
    {
        private readonly ContainerActivator _activator;

        public ContainerCapabilities Capabilities { get; } = ContainerCapabilities.None;

        public StartupContainer()
        {
            _activator = new ContainerActivator(this);
        }

        public object Resolve(Type contractType)
        {
            object result = ResolveOptional(contractType);
            if (result == null)
            {
                throw new InvalidOperationException($"Could not resolve a service with type '{contractType}'.");
            }

            return result;
        }

        public object ResolveOptional(Type contractType)
        {
            if (contractType == typeof(IContainer))
                return this;

            if (contractType == typeof(IActivator))
                return _activator;

            return null;
        }
    }
}
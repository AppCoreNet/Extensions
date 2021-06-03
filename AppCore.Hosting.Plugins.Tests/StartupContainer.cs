// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using System;
using System.Collections.Generic;
using System.Linq;
using AppCore.DependencyInjection;
using AppCore.DependencyInjection.Activator;

namespace AppCore.Hosting.Plugins
{
    internal class StartupContainer : IContainer
    {
        private readonly ContainerActivator _activator;
        private IEnumerable<KeyValuePair<Type, object>> _services;

        public ContainerCapabilities Capabilities { get; } = ContainerCapabilities.None;

        public StartupContainer(IEnumerable<KeyValuePair<Type, object>> services = null)
        {
            _activator = new ContainerActivator(this);
            _services = services ?? Enumerable.Empty<KeyValuePair<Type, object>>();
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

            return _services.FirstOrDefault(kv => kv.Key == contractType).Value;
        }
    }
}
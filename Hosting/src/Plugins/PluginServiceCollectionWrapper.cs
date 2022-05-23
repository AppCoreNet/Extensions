// Licensed under the MIT License.
// Copyright (c) 2018-2022 the AppCore .NET project.

using System.Collections;
using System.Collections.Generic;
using AppCore.Hosting.Plugins;

// ReSharper disable once CheckNamespace
namespace AppCore.DependencyInjection
{
    internal sealed class PluginServiceCollectionWrapper<T> : IPluginServiceCollection<T>
    {
        private readonly IPluginServiceCollection<T> _serviceCollection;

        public PluginServiceCollectionWrapper(IPluginManager pluginManager)
        {
            _serviceCollection = pluginManager.GetServices<T>();
        }

        public IEnumerator<IPluginService<T>> GetEnumerator()
        {
            return _serviceCollection.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
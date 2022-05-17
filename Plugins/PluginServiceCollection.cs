// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace AppCore.Hosting.Plugins
{
    internal class PluginServiceCollection
    {
        private static readonly ConcurrentDictionary<Type, Type> _typeCache = new();

        private class InternalCollection<T> : IInternalPluginServiceCollection<T>
        {
            private readonly List<IPluginService<T>> _instances = new();

            public void Add(IPlugin plugin, object instance)
            {
                _instances.Add(new PluginService<T>(plugin, (T) instance));
            }

            public IEnumerator<IPluginService<T>> GetEnumerator()
            {
                return _instances.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        public static IInternalPluginServiceCollection<object> Create(Type serviceType)
        {
            Type type = _typeCache.GetOrAdd(serviceType, t => typeof(InternalCollection<>).MakeGenericType(serviceType));
            return (IInternalPluginServiceCollection<object>) Activator.CreateInstance(type)!;
        }
    }
}
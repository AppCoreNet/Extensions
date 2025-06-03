// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace AppCoreNet.Extensions.Hosting.Plugins;

internal sealed class PluginServiceCollection
{
    private static readonly ConcurrentDictionary<Type, Type> _typeCache = new();

    private class InternalCollection<T> : IInternalPluginServiceCollection<T>
    {
        private readonly List<IPluginService<T>> _instances = new();

        public void Add(IPlugin plugin, object instance)
        {
            _instances.Add(new PluginService<T>(plugin, (T)instance));
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

    [RequiresUnreferencedCode("Uses reflection to discover services.")]
    [RequiresDynamicCode("Creates the generic plugin service collection.")]
    public static IInternalPluginServiceCollection<object> Create(Type serviceType)
    {
        Type type = _typeCache.GetOrAdd(serviceType, static t => typeof(InternalCollection<>).MakeGenericType(t));
        return (IInternalPluginServiceCollection<object>)Activator.CreateInstance(type)!;
    }
}
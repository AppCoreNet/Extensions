﻿// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System.Collections;
using System.Collections.Generic;
using AppCoreNet.Extensions.Hosting.Plugins;

// ReSharper disable once CheckNamespace
namespace AppCoreNet.Extensions.DependencyInjection;

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
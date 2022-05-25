// Licensed under the MIT License.
// Copyright (c) 2018-2022 the AppCore .NET project.

using System;
using AppCore.Extensions.Hosting.Plugins;

// ReSharper disable once CheckNamespace
namespace AppCore.Extensions.DependencyInjection
{
    internal sealed class PluginServiceWrapper<T> : IPluginService<T>
    {
        private readonly IPluginService<T> _service;

        public T Instance => _service.Instance;

        public IPlugin Plugin => _service.Plugin;

        public PluginServiceWrapper(IPluginManager pluginManager)
        {
            IPluginService<T>? service = pluginManager.GetService<T>();
            _service = service ?? throw new InvalidOperationException(
                $"The service {typeof(T)} is not known to the plugin manager.");
        }
    }
}
// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

namespace AppCore.Extensions.Hosting.Plugins
{
    internal sealed class PluginService<T> : IPluginService<T>
    {
        public T Instance { get; }

        public IPlugin Plugin { get; }

        public PluginService(IPlugin plugin, T value)
        {
            Plugin = plugin;
            Instance = value;
        }
    }
}
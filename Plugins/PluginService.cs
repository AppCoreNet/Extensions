// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

namespace AppCore.Hosting.Plugins
{
    internal class PluginService<T> : IPluginService<T>
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
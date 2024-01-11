// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

namespace AppCore.Extensions.Hosting.Plugins;

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
// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

namespace AppCoreNet.Extensions.Hosting.Plugins;

internal interface IInternalPluginServiceCollection<out T> : IPluginServiceCollection<T>
{
    void Add(IPlugin plugin, object instance);
}
// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

namespace AppCore.Extensions.Hosting.Plugins
{
    internal interface IInternalPluginServiceCollection<out T> : IPluginServiceCollection<T>
    {
        void Add(IPlugin plugin, object instance);
    }
}
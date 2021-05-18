// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

namespace AppCore.Hosting.Plugins
{
    /// <summary>
    /// Represents a plugin service.
    /// </summary>
    public interface IPluginService<out T>
    {
        /// <summary>
        /// Gets the service instance.
        /// </summary>
        T Instance { get; }

        /// <summary>
        /// Gets the plugin instance.,
        /// </summary>
        IPlugin Plugin { get; }
    }
}

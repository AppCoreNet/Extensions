// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using System;
using System.Collections.Generic;

namespace AppCore.Hosting.Plugins
{
    /// <summary>
    /// Provides options for loading plugins.
    /// </summary>
    public class PluginOptions
    {
        /// <summary>
        /// Gets or sets a value indicating whether to resolve private and internal types.
        /// </summary>
        public bool ResolvePrivateTypes { get; set; }

        /// <summary>
        /// Gets or sets the base path when loading plugins with relative paths.
        /// </summary>
        public string PluginBasePath { get; set; } = AppContext.BaseDirectory;

        /// <summary>
        /// Gets the directories that are searched for plugins.
        /// </summary>
        /// <remarks>
        /// Each plugin directory should contain one sub-directory per plugin. The directory
        /// name must be equal to the plugin assembly.
        /// Relative paths are allowed.
        /// </remarks>
        public IList<string> PluginDirectories { get; } = new List<string> { "plugins" };

        /// <summary>
        /// Gets the list of plugin assembly files.
        /// </summary>
        /// <remarks>
        /// Each entry must point to an assembly file path (including the extension .dll).
        /// Relative paths are allowed.
        /// </remarks>
        public IList<string> PluginAssemblies { get; } = new List<string>();
    }
}

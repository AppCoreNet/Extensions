// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

namespace AppCore.Extensions.Hosting.Plugins
{
    /// <summary>
    /// Provides information about a plugin.
    /// </summary>
    public class PluginInfo
    {
        /// <summary>
        /// The title of the plugin.
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// The version of the plugin.
        /// </summary>
        public string? Version { get; }

        /// <summary>
        /// The description of the plugin.
        /// </summary>
        public string? Description { get; }

        /// <summary>
        /// The copyright of the plugin.
        /// </summary>
        public string? Copyright { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginInfo"/> class.
        /// </summary>
        /// <param name="title">The title of the plugin.</param>
        /// <param name="version">The version of the plugin.</param>
        /// <param name="description">The description of the plugin.</param>
        /// <param name="copyright">The copyright of the plugin.</param>
        public PluginInfo(string title, string? version, string? description, string? copyright)
        {
            Version = version;
            Title = title;
            Description = description;
            Copyright = copyright;
        }
    }
}

// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using AppCore.DependencyInjection.Activator;
using AppCore.Diagnostics;
using McMaster.NETCore.Plugins;

namespace AppCore.Hosting.Plugins
{
    /// <summary>
    /// Provides default implementation of the <see cref="IPluginManager"/> interface.
    /// </summary>
    public class PluginManager : IPluginManager
    {
        private readonly IActivator _activator;
        private readonly PluginOptions _options;
        private readonly Lazy<IReadOnlyCollection<IPlugin>> _plugins;

        internal static PluginManager Instance { get; set; }

        internal PluginOptions Options => _options;

        /// <inheritdoc />
        public IReadOnlyCollection<IPlugin> Plugins => _plugins.Value;

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginManager"/> class.
        /// </summary>
        /// <param name="activator"></param>
        /// <param name="options"></param>
        public PluginManager(IActivator activator, PluginOptions options)
        {
            Ensure.Arg.NotNull(activator, nameof(activator));
            Ensure.Arg.NotNull(options, nameof(options));

            _activator = activator;
            _options = options;
            _plugins = new Lazy<IReadOnlyCollection<IPlugin>>(() => LoadPluginsCore().AsReadOnly());
        }

        internal PluginManager(PluginManager parent, IActivator activator, PluginOptions options)
        {
            _activator = activator;
            _options = options;

            if (parent._plugins.IsValueCreated)
            {
                _plugins = new Lazy<IReadOnlyCollection<IPlugin>>(
                    parent._plugins.Value.OfType<Plugin>()
                          .Select(p => new Plugin(p.Loader, activator, options))
                          .ToArray);
            }
            else
            {
                _plugins = new Lazy<IReadOnlyCollection<IPlugin>>(
                    () => LoadPluginsCore().AsReadOnly());
            }
        }

        /// <inheritdoc />
        public IEnumerable<IPluginService<object>> ResolveAll(Type contractType)
        {
            Ensure.Arg.NotNull(contractType, nameof(contractType));
            return Plugins.SelectMany(p => p.ResolveAll(contractType));
        }

        /// <inheritdoc />
        public void LoadPlugins()
        {
            _ = _plugins.Value;
        }

        private List<IPlugin> LoadPluginsCore()
        {
            List<IPlugin> result = new();

            IEnumerable<(string assemblyName, PluginLoader loader)> plugins = GetPluginLoaders();

            foreach ((string assemblyName, PluginLoader loader) plugin in plugins)
            {
                try
                {
                    Debug.WriteLine($"Loading plugin assembly '{plugin.assemblyName}'");
                    result.Add(new Plugin(plugin.loader, _activator, _options));
                }
                catch (Exception error)
                {
                    Console.Error.WriteLine($"Error loading plugin assembly '{plugin.assemblyName}': {error.Message}");
                }
            }

            return result;
        }

        private IEnumerable<(string assemblyName,PluginLoader loader)> GetPluginLoaders()
        {
            // load plugins
            foreach (string plugin in _options.Assemblies)
            {
                string pluginDll = plugin;
                if (!Path.IsPathRooted(pluginDll))
                    pluginDll = Path.GetFullPath(pluginDll, _options.BasePath);

                if (!File.Exists(pluginDll))
                {
                    Console.Error.WriteLine($"Plugin assembly '{pluginDll}' was not found.");
                    continue;
                }

                var loader = PluginLoader.CreateFromAssemblyFile(
                    pluginDll,
                    ConfigurePlugin);

                yield return (pluginDll, loader);
            }

            // find and load plugins in directories
            foreach (string pluginDirectory in _options.Directories)
            {
                string pluginsDir = pluginDirectory;
                if (!Path.IsPathRooted(pluginsDir))
                    pluginsDir = Path.GetFullPath(pluginDirectory, _options.BasePath);

                if (!Directory.Exists(pluginsDir))
                    continue;

                foreach (string dir in Directory.GetDirectories(pluginsDir))
                {
                    string dirName = Path.GetFileName(dir);
                    string pluginDll = Path.Combine(dir, dirName + ".dll");

                    if (File.Exists(pluginDll))
                    {
                        var loader = PluginLoader.CreateFromAssemblyFile(
                            pluginDll,
                            ConfigurePlugin);

                        yield return (pluginDll, loader);
                    }
                    else
                    {
                        Console.Error.WriteLine($"Plugin assembly '{pluginDll}' was not found.");
                    }
                }
            }
        }

        private void ConfigurePlugin(PluginConfig config)
        {
            config.PreferSharedTypes = true;
        }
    }
}
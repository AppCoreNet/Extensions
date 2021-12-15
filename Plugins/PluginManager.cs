// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using AppCore.DependencyInjection.Activator;
using AppCore.Diagnostics;
using McMaster.NETCore.Plugins;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

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
        private static readonly ConcurrentDictionary<Type, Type> _pluginServiceTypeCache = new();

        internal PluginOptions Options => _options;

        /// <inheritdoc />
        public IReadOnlyCollection<IPlugin> Plugins => _plugins.Value;

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginManager"/> class.
        /// </summary>
        /// <param name="activator"></param>
        /// <param name="options"></param>
        public PluginManager(IActivator activator, IOptions<PluginOptions> options)
        {
            Ensure.Arg.NotNull(activator, nameof(activator));
            Ensure.Arg.NotNull(options, nameof(options));

            _activator = activator;
            _options = options.Value;
            _plugins = new Lazy<IReadOnlyCollection<IPlugin>>(() => LoadPluginsCore().AsReadOnly());
        }

        internal PluginManager(PluginManager parent, IActivator activator, IOptions<PluginOptions> options)
        {
            _activator = activator;
            _options = options.Value;

            if (parent._plugins.IsValueCreated)
            {
                _plugins = new Lazy<IReadOnlyCollection<IPlugin>>(
                    parent._plugins.Value.OfType<Plugin>()
                          .Select(p => new Plugin(p.Loader, activator, _options))
                          .ToArray);
            }
            else
            {
                _plugins = new Lazy<IReadOnlyCollection<IPlugin>>(
                    () => LoadPluginsCore().AsReadOnly());
            }
        }

        /// <inheritdoc />
        public IPluginServiceCollection<object> GetServices(Type serviceType)
        {
            Ensure.Arg.NotNull(serviceType, nameof(serviceType));

            IInternalPluginServiceCollection<object> result = PluginServiceCollection.Create(serviceType);
            foreach (IPlugin plugin in Plugins)
            {
                foreach (object service in plugin.GetServices(serviceType))
                {
                    result.Add(plugin, service);
                }
            }

            return result;
        }

        /// <inheritdoc />
        public IPluginService<object> GetService(Type serviceType)
        {
            Type pluginServiceType = _pluginServiceTypeCache.GetOrAdd(
                serviceType,
                t => typeof(PluginService<>).MakeGenericType(t));

            IPluginService<object> result = null;

            foreach (IPlugin plugin in Plugins)
            {
                object service = plugin.GetService(serviceType);
                if (service != null)
                {
                    result = (IPluginService<object>) Activator.CreateInstance(pluginServiceType, plugin, service);
                }
            }

            return result;
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
            PluginLoader GetPluginLoader(string assemblyPath)
            {
                if (!File.Exists(assemblyPath))
                {
                    Console.Error.WriteLine($"Plugin assembly '{assemblyPath}' was not found.");
                    return null;
                }

                string pluginName = Path.GetFileNameWithoutExtension(assemblyPath);
                if (_options.Disabled.Contains(pluginName, StringComparer.OrdinalIgnoreCase))
                    return null;

                return PluginLoader.CreateFromAssemblyFile(assemblyPath, ConfigurePlugin);
            }

            // load plugins
            foreach (string plugin in _options.Assemblies)
            {
                string pluginDll = plugin;
                if (!Path.IsPathRooted(pluginDll))
                    pluginDll = Path.GetFullPath(pluginDll, _options.BasePath);

                PluginLoader loader = GetPluginLoader(pluginDll);
                if (loader != null)
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

                    PluginLoader loader = GetPluginLoader(pluginDll);
                    if (loader != null)
                        yield return (pluginDll, loader);
                }
            }
        }

        private void ConfigurePlugin(PluginConfig config)
        {
            config.PreferSharedTypes = true;
        }
    }
}
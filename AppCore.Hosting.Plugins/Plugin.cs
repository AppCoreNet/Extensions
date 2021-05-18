// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using System;
using System.Collections.Generic;
using System.Reflection;
using AppCore.DependencyInjection;
using AppCore.DependencyInjection.Activator;
using AppCore.Diagnostics;
using McMaster.NETCore.Plugins;

namespace AppCore.Hosting.Plugins
{
    internal class Plugin : IPlugin
    {
        private readonly IActivator _activator;
        private readonly PluginOptions _options;

        public PluginLoader Loader { get; }

        public PluginInfo Info
        {
            get
            {
                var assemblyInfo = new AssemblyInfo(Assembly);
                return new PluginInfo(
                    assemblyInfo.Title,
                    assemblyInfo.Version,
                    assemblyInfo.Description,
                    assemblyInfo.Copyright);
            }
        }

        public Assembly Assembly { get; }

        public Plugin(PluginLoader loader, IActivator activator, PluginOptions options)
        {
            _activator = activator;
            _options = options;

            Loader = loader;
            Assembly = loader.LoadDefaultAssembly();
        }

        public IEnumerable<IPluginService<object>> ResolveAll(Type contractType)
        {
            Ensure.Arg.NotNull(contractType, nameof(contractType));

            var scanner = new AssemblyScanner(contractType, new[] { Assembly });
            scanner.IncludePrivateTypes = _options.ResolvePrivateTypes;
            scanner.Filters.Clear();
            foreach (Type type in scanner.ScanAssemblies())
            {
                object instance;
                try
                {
                    instance = _activator.CreateInstance(type);
                }
                catch (Exception error)
                {
                    Console.Error.WriteLine($"Error activating plugin type '{type.FullName}': {error.Message}");
                    continue;
                }

                Type pluginServiceType = typeof(PluginService<>).MakeGenericType(instance.GetType());
                yield return (IPluginService<object>) Activator.CreateInstance(pluginServiceType, this, instance);
            }
        }

        public IDisposable EnterContextualReflection()
        {
            return Loader.EnterContextualReflection();
        }
    }
}
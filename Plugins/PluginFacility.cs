// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AppCore.DependencyInjection.Activator;
using AppCore.DependencyInjection.Facilities;
using AppCore.Diagnostics;
using AppCore.Hosting.Plugins;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

// ReSharper disable once CheckNamespace
namespace AppCore.DependencyInjection
{
    public class PluginFacility : Facility
    {
        private readonly IActivator _activator;
        private readonly List<IConfigureOptions<PluginOptions>> _configureOptions = new();

        internal static PluginManager PluginManager { get; set; }
        
        public PluginFacility(IActivator activator)
        {
            Ensure.Arg.NotNull(activator, nameof(activator));
            _activator = activator;
        }

        public PluginFacility Configure(Action<PluginOptions> configure)
        {
            _configureOptions.Add(new ConfigureOptions<PluginOptions>(configure));
            ConfigureServices(s => s.Configure(configure));
            return this;
        }

        private IOptions<PluginOptions> CreateOptions()
        {
            var optionsManager = new OptionsManager<PluginOptions>(
                new OptionsFactory<PluginOptions>(
                    _configureOptions,
                    Enumerable.Empty<IPostConfigureOptions<PluginOptions>>()));

            return optionsManager;
        }

        /// <inheritdoc />
        protected override void ConfigureServices(IServiceCollection services)
        {
            PluginManager ??= new PluginManager(_activator, CreateOptions());

            base.ConfigureServices(services);

            services.TryAddSingleton<IPluginManager>(
                sp => new PluginManager(
                    PluginManager,
                    sp.GetRequiredService<IActivator>(),
                    sp.GetRequiredService<IOptions<PluginOptions>>()));

            services.TryAddTransient(typeof(IPluginService<>), typeof(PluginServiceWrapper<>));
            services.TryAddTransient(typeof(IPluginServiceCollection<>), typeof(PluginServiceCollectionWrapper<>));
        }

        private class PluginServiceWrapper<T> : IPluginService<T>
        {
            private readonly IPluginService<T> _service;

            public T Instance => _service.Instance;

            public IPlugin Plugin => _service.Plugin;

            public PluginServiceWrapper(IPluginManager pluginManager)
            {
                _service = pluginManager.GetService<T>();
            }
        }

        private class PluginServiceCollectionWrapper<T> : IPluginServiceCollection<T>
        {
            private readonly IPluginServiceCollection<T> _serviceCollection;

            public PluginServiceCollectionWrapper(IPluginManager pluginManager)
            {
                _serviceCollection = pluginManager.GetServices<T>();
            }

            public IEnumerator<IPluginService<T>> GetEnumerator()
            {
                return _serviceCollection.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
    }
}

// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using System;
using System.Collections.Generic;
using AppCore.DependencyInjection;
using AppCore.DependencyInjection.Activator;
using AppCore.DependencyInjection.Facilities;
using AppCore.Diagnostics;

namespace AppCore.Hosting.Plugins
{
    public class PluginHostingFacility : Facility
    {
        private readonly IActivator _activator;
        private List<Action<PluginOptions>> _configureActions = new();

        public PluginHostingFacility(IActivator activator)
        {
            Ensure.Arg.NotNull(activator, nameof(activator));
            _activator = activator;
        }

        public PluginHostingFacility Configure(Action<PluginOptions> configure)
        {
            _configureActions.Add(configure);
            return this;
        }

        private PluginOptions CreateOptions()
        {
            var options = new PluginOptions();
            foreach (Action<PluginOptions> configureAction in _configureActions)
                configureAction(options);

            return options;
        }

        /// <inheritdoc />
        protected override void Build(IComponentRegistry registry)
        {
            PluginManager.Instance ??= new PluginManager(_activator, CreateOptions());

            base.Build(registry);

            registry.TryAdd(
                ComponentRegistration.Singleton<IPluginManager>(
                    ComponentFactory.Create(
                        c => new PluginManager(
                            PluginManager.Instance,
                            c.Resolve<IActivator>(),
                            CreateOptions())
                    )
                ));
        }
    }
}

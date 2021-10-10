// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using AppCore.DependencyInjection.Activator;
using AppCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace AppCore.DependencyInjection.Facilities
{
    /// <summary>
    /// Represents a facility.
    /// </summary>
    public abstract class Facility
    {
        private class ExtensionCallback
        {
            public Action<FacilityExtension> Configure { get; }

            public bool Invoked { get; set; }

            public ExtensionCallback(Action<FacilityExtension> configure)
            {
                Configure = configure;
            }
        }

        private readonly List<Action<IServiceCollection>> _callbacks = new();
        private readonly Dictionary<Type, List<ExtensionCallback>> _extensionTypes = new();
        private readonly List<FacilityExtension> _extensions = new();
        private bool _extensionsChanged;

        /// <summary>
        /// Registers a callback which is invoked when the services are configured.
        /// </summary>
        /// <param name="callback">The callback.</param>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected void ConfigureServices(Action<IServiceCollection> callback)
        {
            Ensure.Arg.NotNull(callback, nameof(callback));
            _callbacks.Add(callback);
        }

        /// <summary>
        /// Adds an extension to the facility.
        /// </summary>
        /// <param name="extensionType">The type of the facility extension.</param>
        /// <param name="configure">The configuration delegate.</param>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public void AddExtension(Type extensionType, Action<FacilityExtension> configure = null)
        {
            Ensure.Arg.NotNull(extensionType, nameof(extensionType));
            Ensure.Arg.OfType(extensionType, typeof(FacilityExtension), nameof(extensionType));

            _extensionsChanged = true;
            if (!_extensionTypes.TryGetValue(extensionType, out List<ExtensionCallback> callbacks))
            {
                _extensionTypes.Add(
                    extensionType,
                    new List<ExtensionCallback>(new[] { new ExtensionCallback(configure) }));
            }
            else
            {
                callbacks.Add(new ExtensionCallback(configure));
            }
        }

        /// <summary>
        /// Adds an extension to the facility.
        /// </summary>
        /// <typeparam name="T">The type of the facility extension.</typeparam>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public void AddExtension<T>(Action<T> configure = null)
            where T : FacilityExtension
        {
            AddExtension(typeof(T), f => configure?.Invoke((T) f));
        }

        private void ConfigureExtensions(IActivator activator)
        {
            while (_extensionsChanged)
            {
                _extensionsChanged = false;

                foreach (KeyValuePair<Type, List<ExtensionCallback>> extensionTypeCallback in _extensionTypes.ToArray())
                {
                    FacilityExtension extension =
                        _extensions.FirstOrDefault(e => e.GetType() == extensionTypeCallback.Key);

                    if (extension == null)
                    {
                        extension = (FacilityExtension)activator.CreateInstance(extensionTypeCallback.Key);
                        extension.Facility = this;
                        _extensions.Add(extension);
                    }

                    foreach (ExtensionCallback extensionCallback in extensionTypeCallback.Value.ToArray())
                    {
                        if (!extensionCallback.Invoked)
                        {
                            extensionCallback.Configure?.Invoke(extension);
                            extensionCallback.Invoked = true;
                        }
                    }
                }
            }
        }

        internal void ConfigureServices(IActivator activator, IServiceCollection services)
        {
            ConfigureExtensions(activator);
            ConfigureServices(services);
        }

        /// <summary>
        /// Can be overridden to register services with the <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        protected virtual void ConfigureServices(IServiceCollection services)
        {
            foreach (Action<IServiceCollection> callback in _callbacks)
            {
                callback(services);
            }

            foreach (FacilityExtension extension in _extensions)
            {
                extension.ConfigureServices(services);
            }
        }
    }
}
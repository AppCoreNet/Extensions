// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        private readonly List<Action<IServiceCollection>> _callbacks = new();
        private readonly Dictionary<Type, List<Action<FacilityExtension>>> _extensions = new();

        /// <summary>
        /// Registers a callback which is invoked when the facility is built.
        /// </summary>
        /// <param name="callback">The callback.</param>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public void AddCallback(Action<IServiceCollection> callback)
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

            if (!_extensions.TryGetValue(extensionType, out List<Action<FacilityExtension>> callbacks))
            {
                _extensions.Add(extensionType, new List<Action<FacilityExtension>>(new []{configure}));
            }
            else
            {
                callbacks.Add(configure);
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

        internal void ConfigureServices(IActivator activator, IServiceCollection services)
        {
            foreach (Action<IServiceCollection> callback in _callbacks)
            {
                callback(services);
            }

            foreach (KeyValuePair<Type, List<Action<FacilityExtension>>> extensionTypeCallback in _extensions)
            {
                var extension = (FacilityExtension) activator.CreateInstance(extensionTypeCallback.Key);
                extension.Facility = this;
                foreach (Action<FacilityExtension> extensionCallback in extensionTypeCallback.Value)
                {
                    extensionCallback?.Invoke(extension);
                }

                extension.ConfigureServices(services);
            }

            ConfigureServices(services);
        }

        /// <summary>
        /// Can be overridden to register services with the <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        protected virtual void ConfigureServices(IServiceCollection services)
        {
        }
    }
}
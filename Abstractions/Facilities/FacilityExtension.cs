// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using AppCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace AppCore.DependencyInjection.Facilities
{
    /// <summary>
    /// Represents an extension for a facility.
    /// </summary>
    public abstract class FacilityExtension
    {
        private readonly List<Action<IServiceCollection>> _callbacks = new();

        /// <summary>
        /// Gets the <see cref="Facility"/> of the extensions.
        /// </summary>
        public Facility? Facility { get; internal set; }

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
        /// Must be overridden to register services with the <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        protected internal virtual void ConfigureServices(IServiceCollection services)
        {
            foreach (Action<IServiceCollection> callback in _callbacks)
            {
                callback(services);
            }
        }
    }
}
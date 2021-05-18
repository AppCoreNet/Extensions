// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using AppCore.Diagnostics;

namespace AppCore.DependencyInjection.Facilities
{
    /// <summary>
    /// Represents an extension for a facility.
    /// </summary>
    public abstract class FacilityExtension
    {
        private readonly List<Action<IComponentRegistry>> _registrations = new();

        /// <summary>
        /// Gets the <see cref="Facility"/> of the extensions.
        /// </summary>
        public Facility Facility { get; internal set; }

        /// <summary>
        /// Registers a callback which is invoked when the facility is built.
        /// </summary>
        /// <param name="callback">The callback.</param>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public void ConfigureRegistry(Action<IComponentRegistry> callback)
        {
            Ensure.Arg.NotNull(callback, nameof(callback));
            _registrations.Add(callback);
        }

        /// <summary>
        /// Must be overridden to register components with the <see cref="IComponentRegistry"/>.
        /// </summary>
        /// <param name="registry">The <see cref="IComponentRegistry"/>.</param>
        protected internal virtual void Build(IComponentRegistry registry)
        {
            foreach (Action<IComponentRegistry> registration in _registrations)
            {
                registration(registry);
            }
        }
    }
}
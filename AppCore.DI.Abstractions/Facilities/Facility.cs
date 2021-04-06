// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using AppCore.Diagnostics;

namespace AppCore.DependencyInjection.Facilities
{
    /// <summary>
    /// Represents a facility.
    /// </summary>
    public abstract class Facility
    {
        private readonly List<Action<IComponentRegistry>> _registrations = new();
        private static IFacilityActivator _activator = DefaultFacilityActivator.Instance;

        /// <summary>
        /// Gets or sets the activator for facilities.
        /// </summary>
        public static IFacilityActivator Activator
        {
            get => _activator;
            set
            {
                Ensure.Arg.NotNull(value, nameof(value));
                _activator = value;
            }
        }

        /// <summary>
        /// Registers a callback which is invoked when the facility is built.
        /// </summary>
        /// <param name="callback">The callback.</param>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public void Register(Action<IComponentRegistry> callback)
        {
            Ensure.Arg.NotNull(callback, nameof(callback));
            _registrations.Add(callback);
        }

        /// <summary>
        /// Can be overridden to register components with the <see cref="IComponentRegistry"/>.
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
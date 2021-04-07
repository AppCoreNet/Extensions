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
        private static IFacilityActivator _activator = DefaultFacilityActivator.Instance;
        private readonly List<Action<IComponentRegistry>> _registrations = new();
        private readonly List<FacilityExtension> _extensions = new();

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
        /// Adds an extension to the facility.
        /// </summary>
        /// <param name="extensionType">The type of the facility extension.</param>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public void AddExtension(Type extensionType)
        {
            Ensure.Arg.NotNull(extensionType, nameof(extensionType));
            Ensure.Arg.OfType(extensionType, typeof(FacilityExtension), nameof(extensionType));

            _extensions.Add((FacilityExtension) Activator.CreateInstance(extensionType));
        }

        /// <summary>
        /// Adds an extension to the facility.
        /// </summary>
        /// <typeparam name="T">The type of the facility extension.</typeparam>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public void AddExtension<T>()
            where T : FacilityExtension
        {
            AddExtension(typeof(T));
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

            foreach (FacilityExtension extension in _extensions)
            {
                extension.Build(registry);
            }
        }
    }
}
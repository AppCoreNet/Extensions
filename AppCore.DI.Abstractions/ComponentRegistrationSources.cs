// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using AppCore.Diagnostics;

namespace AppCore.DependencyInjection
{
    /// <summary>
    /// Provides a default implementation of the y<see cref="IComponentRegistrationSources"/> interface.
    /// </summary>
    public class ComponentRegistrationSources : IComponentRegistrationSources
    {
        private readonly List<IComponentRegistrationSource> _sources = new();
        private readonly Type _contractType;
        private readonly ComponentLifetime _defaultLifetime;

        /// <summary>
        /// Initializes a new instance of the <see cref="ComponentRegistrationSources"/> class.
        /// </summary>
        public ComponentRegistrationSources(
            Type contractType = null,
            ComponentLifetime defaultLifetime = ComponentLifetime.Transient)
        {
            _contractType = contractType;
            _defaultLifetime = defaultLifetime;
        }

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public IComponentRegistrationSources Add(IComponentRegistrationSource source)
        {
            Ensure.Arg.NotNull(source, nameof(source));

            // configure the source
            source.WithDefaultLifetime(_defaultLifetime);
            if (_contractType != null)
                source.WithContract(_contractType);

            _sources.Add(source);
            return this;
        }

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public IComponentRegistrationSources Add<T>(Action<T> configure = null)
            where T : IComponentRegistrationSource, new()
        {
            var source = new T();
            Add(source);
            configure?.Invoke(source);
            return this;
        }

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public IEnumerable<ComponentRegistration> GetRegistrations()
        {
            return _sources.SelectMany(s => s.GetRegistrations());
        }
    }
}
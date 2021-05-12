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
        private readonly Type _contractType;
        private readonly List<IComponentRegistrationSource> _sources = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="ComponentRegistrationSources"/> class.
        /// </summary>
        public ComponentRegistrationSources()
        : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ComponentRegistrationSources"/> class.
        /// </summary>
        /// <param name="contractType">The contract of the registered components.</param>
        public ComponentRegistrationSources(Type contractType)
        {
            _contractType = contractType;
        }

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public IComponentRegistrationSources Add<T>(Action<T> configure)
            where T : IComponentRegistrationSource, new()
        {
            Ensure.Arg.NotNull(configure, nameof(configure));

            var source = new T();
            if (_contractType != null)
                source.WithContract(_contractType);

            configure(source);
            _sources.Add(source);

            return this;
        }

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public IEnumerable<ComponentRegistration> BuildRegistrations()
        {
            return _sources.SelectMany(s => s.BuildRegistrations());
        }
    }
}
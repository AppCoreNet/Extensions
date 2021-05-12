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
        private Type _contractType;
        private ComponentLifetime _defaultLifetime;

        /// <summary>
        /// Initializes a new instance of the <see cref="ComponentRegistrationSources"/> class.
        /// </summary>
        public ComponentRegistrationSources()
        {
        }

        /// <summary>
        /// Sets the contract type which is being registered.
        /// </summary>
        /// <param name="contractType">The type of the contract.</param>
        /// <returns>The <see cref="ComponentRegistrationSources"/>.</returns>
        public ComponentRegistrationSources WithContract(Type contractType)
        {
            Ensure.Arg.NotNull(contractType, nameof(contractType));
            _contractType = contractType;
            return this;
        }

        /// <summary>
        /// Sets the contract type which is being registered.
        /// </summary>
        /// <typeparam name="TContract">The type of the contract.</typeparam>
        /// <returns>The <see cref="ComponentRegistrationSources"/>.</returns>
        public ComponentRegistrationSources WithContract<TContract>()
            where TContract : class
        {
            return WithContract(typeof(TContract));
        }

        /// <summary>
        /// Specifies the default lifetime for components.
        /// </summary>
        /// <param name="lifetime">The default lifetime.</param>
        /// <returns>The <see cref="ComponentRegistrationSources"/>.</returns>
        public ComponentRegistrationSources WithDefaultLifetime(ComponentLifetime lifetime)
        {
            _defaultLifetime = lifetime;
            return this;
        }

        /// <inheritdoc />
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
        public IEnumerable<ComponentRegistration> BuildRegistrations()
        {
            return _sources.SelectMany(s => s.BuildRegistrations());
        }
    }
}
// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using AppCore.Diagnostics;

namespace AppCore.DependencyInjection.Facilities
{
    /// <summary>
    /// Provides a default implementation of the y<see cref="IFacilityRegistrationSources"/> interface.
    /// </summary>
    public class FacilityRegistrationSources : IFacilityRegistrationSources
    {
        private readonly List<IFacilityRegistrationSource> _sources = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="FacilityRegistrationSources"/> class.
        /// </summary>
        public FacilityRegistrationSources()
        {
        }

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public IFacilityRegistrationSources Add(IFacilityRegistrationSource source)
        {
            Ensure.Arg.NotNull(source, nameof(source));
            _sources.Add(source);
            return this;
        }

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public IFacilityRegistrationSources Add<T>(Action<T> configure = null)
            where T : IFacilityRegistrationSource, new()
        {
            var source = new T();
            Add(source);
            configure?.Invoke(source);
            return this;
        }

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public IEnumerable<Facility> GetFacilities()
        {
            return _sources.SelectMany(s => s.GetFacilities());
        }
    }
}
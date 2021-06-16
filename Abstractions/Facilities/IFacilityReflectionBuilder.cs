// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using System;
using System.ComponentModel;

namespace AppCore.DependencyInjection.Facilities
{
    /// <summary>
    /// Represents a builder for resolving facilities by reflection.
    /// </summary>
    public interface IFacilityReflectionBuilder
    {
        /// <summary>
        /// Adds a facility resolver to the builder.
        /// </summary>
        /// <param name="resolver">The facility resolver.</param>
        /// <returns>The <see cref="IFacilityReflectionBuilder"/> to allow chaining.</returns>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        IFacilityReflectionBuilder AddResolver(IFacilityResolver resolver);

        /// <summary>
        /// Adds a facility resolver to the builder.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IFacilityReflectionBuilder"/>.</typeparam>
        /// <param name="configure">The configuration delegate.</param>
        /// <returns>The <see cref="IFacilityReflectionBuilder"/> to allow chaining.</returns>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public IFacilityReflectionBuilder AddResolver<T>(Action<T> configure = null)
            where T : IFacilityResolver;
    }
}
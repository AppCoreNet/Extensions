// Licensed under the MIT License.
// Copyright (c) 2018 the AppCore .NET project.

namespace AppCore.DependencyInjection.Facilities
{
    /// <summary>
    /// Abstract base class for facility extensions.
    /// </summary>
    /// <typeparam name="TFacility">The type of the facility which is extended.</typeparam>
    /// <seealso cref="IFacility"/>
    /// <seealso cref="IFacilityExtension{TFacility}"/>
    public abstract class FacilityExtension<TFacility> : IFacilityExtension<TFacility>
        where TFacility : IFacility
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FacilityExtension{TFacility}"/> class.
        /// </summary>
        protected FacilityExtension()
        {
        }

        /// <summary>
        /// Must be implemented to register the components of the facility extension.
        /// </summary>
        /// <param name="registry">The <see cref="IComponentRegistry"/> where components are registered.</param>
        /// <param name="facility">The <see cref="IFacility"/> for which components are registered.</param>
        protected abstract void RegisterComponents(IComponentRegistry registry, TFacility facility);

        void IFacilityExtension<TFacility>.RegisterComponents(IComponentRegistry registry, TFacility facility)
        {
            RegisterComponents(registry, facility);
        }
    }
}
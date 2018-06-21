// Licensed under the MIT License.
// Copyright (c) 2018 the AppCore .NET project.

namespace AppCore.DependencyInjection
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

        protected abstract void RegisterComponents(IComponentRegistry registry, TFacility facility);

        void IFacilityExtension<TFacility>.RegisterComponents(IComponentRegistry registry, TFacility facility)
        {
            RegisterComponents(registry, facility);
        }
    }
}
// Licensed under the MIT License.
// Copyright (c) 2018,2019 the AppCore .NET project.

using System.Collections.Generic;

namespace AppCore.DependencyInjection.Facilities
{
    /// <summary>
    /// Abstract base class for the <see cref="IFacility"/> type.
    /// </summary>
    /// <seealso cref="IFacility"/>
    public abstract class Facility : IFacility
    {
        /// <inheritdoc />
        public IList<IFacilityExtension> Extensions { get; } = new List<IFacilityExtension>();

        /// <summary>
        /// Initializes a new instance of the <see cref="Facility"/> class.
        /// </summary>
        protected Facility()
        {
        }

        /// <summary>
        /// Must be implemented to register the components of the facility.
        /// </summary>
        /// <param name="registry">The <see cref="IComponentRegistry"/> where components are registered.</param>
        protected abstract void RegisterComponents(IComponentRegistry registry);

        /// <summary>
        /// Can be overridden to register the components of the facility extensions.
        /// </summary>
        /// <param name="registry">The <see cref="IComponentRegistry"/> where components are registered.</param>
        protected virtual void RegisterExtensionComponents(IComponentRegistry registry)
        {
            foreach (IFacilityExtension extension in Extensions)
            {
                extension.RegisterComponents(registry, this);
            }
        }

        /// <summary>
        /// Can be overridden to register the components of the facility and its extensions.
        /// </summary>
        /// <param name="registry">The <see cref="IComponentRegistry"/> where components are registered.</param>
        protected virtual void RegisterComponentsCore(IComponentRegistry registry)
        {
            RegisterComponents(registry);
            RegisterExtensionComponents(registry);
        }

        void IFacility.RegisterComponents(IComponentRegistry registry)
        {
            RegisterComponentsCore(registry);
        }
    }
}
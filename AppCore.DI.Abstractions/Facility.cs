// Licensed under the MIT License.
// Copyright (c) 2018 the AppCore .NET project.

namespace AppCore.DependencyInjection
{
    /// <summary>
    /// Abstract base class for the <see cref="IFacility"/> type.
    /// </summary>
    /// <seealso cref="IFacility"/>
    public abstract class Facility : IFacility
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Facility"/> class.
        /// </summary>
        protected Facility()
        {
        }

        protected abstract void RegisterComponents(IComponentRegistry registry);

        void IFacility.RegisterComponents(IComponentRegistry registry)
        {
            RegisterComponents(registry);
        }
    }
}
// Licensed under the MIT License.
// Copyright (c) 2018 the AppCore .NET project.

using System;
using System.Collections.Generic;
using AppCore.DependencyInjection.Builder;

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
        private readonly FacilityComponentRegistry _registry = new FacilityComponentRegistry();

        protected IComponentRegistry Registry => _registry;

        /// <summary>
        /// Initializes a new instance of the <see cref="FacilityExtension{TFacility}"/> class.
        /// </summary>
        protected FacilityExtension()
        {
        }

        protected void Register(ComponentRegistration registration)
        {
            _registry.Register(registration);
        }

        protected IRegistrationBuilder Register(Type contractType)
        {
            return _registry.Register(contractType);
        }

        protected IRegistrationBuilder<TContract> Register<TContract>()
        {
            return _registry.Register<TContract>();
        }

        protected virtual void RegisterComponents(TFacility facility)
        {
        }

        IEnumerable<ComponentRegistration> IFacilityExtension<TFacility>.GetComponentRegistrations(TFacility facility)
        {
            RegisterComponents(facility);
            return _registry.GetComponentRegistrations();
        }
    }
}
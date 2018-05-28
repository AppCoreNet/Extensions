// Licensed under the MIT License.
// Copyright (c) 2018 the AppCore .NET project.

using System;
using System.Collections.Generic;
using AppCore.DependencyInjection.Builder;

namespace AppCore.DependencyInjection
{
    /// <summary>
    /// Abstract base class for the <see cref="IFacility"/> type.
    /// </summary>
    /// <seealso cref="IFacility"/>
    public abstract class Facility : IFacility
    {
        private readonly FacilityComponentRegistry _registry = new FacilityComponentRegistry();

        protected IComponentRegistry Registry => _registry;

        /// <summary>
        /// Initializes a new instance of the <see cref="Facility"/> class.
        /// </summary>
        protected Facility()
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

        protected virtual void RegisterComponents()
        {
        }

        IEnumerable<ComponentRegistration> IFacility.GetComponentRegistrations()
        {
            RegisterComponents();
            return _registry.GetComponentRegistrations();
        }
    }
}
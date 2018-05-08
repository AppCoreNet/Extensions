// Copyright 2018 the AppCore project.
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated
// documentation files (the "Software"), to deal in the Software without restriction, including without limitation
// the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and
// to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or substantial portions
// of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
// BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System.Collections.Generic;
using AppCore.Diagnostics;

namespace AppCore.DependencyInjection
{
    /// <summary>
    /// Represents a type to configure facilities.
    /// </summary>
    public class FacilityBuilder : IFacilityBuilder
    {
        private class ComponentRegistry : IComponentRegistry
        {
            private readonly List<ComponentRegistration> _components = new List<ComponentRegistration>();

            private readonly List<ComponentAssemblyRegistration> _componentAssemblies =
                new List<ComponentAssemblyRegistration>();

            public void Register(ComponentRegistration registration)
            {
                _components.Add(registration);
            }

            public void RegisterAssembly(ComponentAssemblyRegistration registration)
            {
                _componentAssemblies.Add(registration);
            }

            public void RegisterComponents(IComponentRegistry registry)
            {
                foreach (ComponentRegistration component in _components)
                {
                    registry.Register(component);
                }

                foreach (ComponentAssemblyRegistration componentAssembly in _componentAssemblies)
                {
                    registry.RegisterAssembly(componentAssembly);
                }
            }
        }

        private readonly ComponentRegistry _registry = new ComponentRegistry();
        private readonly IFacility _facility;

        IFacility IFacilityBuilder.Facility => _facility;

        IComponentRegistry IFacilityBuilder.Registry => _registry;

        /// <summary>
        /// Initializes a new instance of the <see cref="FacilityBuilder"/> class.
        /// </summary>
        /// <param name="facility">The <see cref="IFacility"/> which is configured.</param>
        public FacilityBuilder(IFacility facility)
        {
            Ensure.Arg.NotNull(facility, nameof(facility));

            _facility = facility;
        }

        /// <inheritdoc />
        void IFacilityBuilder.RegisterComponents(IComponentRegistry registry)
        {
            RegisterComponents(registry);
        }

        /// <summary>
        /// Registers all components of the facility with the given <see cref="IComponentRegistry"/>.
        /// </summary>
        /// <param name="registry">The <see cref="IComponentRegistry"/>.</param>
        protected virtual void RegisterComponents(IComponentRegistry registry)
        {
            Ensure.Arg.NotNull(registry, nameof(registry));

            _facility.RegisterComponents(registry);
            _registry.RegisterComponents(registry);
        }
    }

    /// <summary>
    /// Represents a type to configure a facility.
    /// </summary>
    /// <typeparam name="TFacility">The type of the <see cref="IFacility"/>.</typeparam>
    public class FacilityBuilder<TFacility> : FacilityBuilder, IFacilityBuilder<TFacility>
        where TFacility : IFacility
    {
        private readonly List<IFacilityExtension<TFacility>> _extensions = new List<IFacilityExtension<TFacility>>();

        TFacility IFacilityBuilder<TFacility>.Facility => (TFacility) ((IFacilityBuilder)this).Facility;

        /// <summary>
        /// Initializes a new instance of the <see cref="FacilityBuilder{TFacility}"/> class.
        /// </summary>
        /// <param name="facility">The <see cref="IFacility"/> to configure.</param>
        public FacilityBuilder(TFacility facility)
            : base(facility)
        {
        }

        /// <summary>
        /// Configures the facility to use the given <see cref="IFacilityExtension{TFacility}"/>.
        /// </summary>
        /// <param name="extension">The <see cref="IFacilityExtension{TFacility}"/> added to the facility.</param>
        public void UseExtension(IFacilityExtension<TFacility> extension)
        {
            Ensure.Arg.NotNull(extension, nameof(extension));

            _extensions.Add(extension);
        }

        /// <summary>
        /// Registers all components of the facility with the given <see cref="IComponentRegistry"/>.
        /// </summary>
        /// <param name="registry">The <see cref="IComponentRegistry"/>.</param>
        protected override void RegisterComponents(IComponentRegistry registry)
        {
            base.RegisterComponents(registry);

            foreach (IFacilityExtension<TFacility> extension in _extensions)
            {
                extension.RegisterComponents(registry, ((IFacilityBuilder<TFacility>)this).Facility);
            }
        }
    }
}
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
        private class ServiceRegistrationList : IServiceRegistrar
        {
            private readonly List<ServiceRegistration> _services = new List<ServiceRegistration>();

            private readonly List<AssemblyRegistration> _serviceAssemblies =
                new List<AssemblyRegistration>();

            public void Register(ServiceRegistration registration)
            {
                _services.Add(registration);
            }

            public void RegisterAssembly(AssemblyRegistration registration)
            {
                _serviceAssemblies.Add(registration);
            }

            public void RegisterServices(IServiceRegistrar registrar)
            {
                foreach (ServiceRegistration service in _services)
                {
                    registrar.Register(service);
                }

                foreach (AssemblyRegistration serviceAssembly in _serviceAssemblies)
                {
                    registrar.RegisterAssembly(serviceAssembly);
                }
            }
        }

        private readonly ServiceRegistrationList _registrar = new ServiceRegistrationList();
        private readonly IFacility _facility;

        IFacility IFacilityBuilder.Facility => _facility;

        IServiceRegistrar IFacilityBuilder.Registrar => _registrar;

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
        void IFacilityBuilder.RegisterServices(IServiceRegistrar registrar)
        {
            RegisterServices(registrar);
        }

        /// <summary>
        /// Registers all services of the facility with the given <see cref="IServiceRegistrar"/>.
        /// </summary>
        /// <param name="registrar">The <see cref="IServiceRegistrar"/>.</param>
        protected virtual void RegisterServices(IServiceRegistrar registrar)
        {
            Ensure.Arg.NotNull(registrar, nameof(registrar));

            _facility.RegisterServices(registrar);
            _registrar.RegisterServices(registrar);
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
        /// Registers all services of the facility with the given <see cref="IServiceRegistrar"/>.
        /// </summary>
        /// <param name="registrar">The <see cref="IServiceRegistrar"/>.</param>
        protected override void RegisterServices(IServiceRegistrar registrar)
        {
            base.RegisterServices(registrar);

            foreach (IFacilityExtension<TFacility> extension in _extensions)
            {
                extension.RegisterServices(registrar, ((IFacilityBuilder<TFacility>)this).Facility);
            }
        }
    }
}
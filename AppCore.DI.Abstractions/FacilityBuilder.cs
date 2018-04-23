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
    public class FacilityBuilder : IFacilityBuilder
    {
        private class ServiceRegistrationList : List<ServiceRegistration>, IServiceRegistrar
        {
            public void Register(ServiceRegistration registration)
            {
                Add(registration);
            }
        }

        private readonly ServiceRegistrationList _registrar = new ServiceRegistrationList();
        private readonly IFacility _facility;

        IFacility IFacilityBuilder.Facility => _facility;

        IServiceRegistrar IFacilityBuilder.Registrar => _registrar;

        public FacilityBuilder(IFacility facility)
        {
            Ensure.Arg.NotNull(facility, nameof(facility));

            _facility = facility;
        }

        void IFacilityBuilder.RegisterServices(IServiceRegistrar registrar)
        {
            RegisterServices(registrar);
        }

        protected virtual void RegisterServices(IServiceRegistrar registrar)
        {
            Ensure.Arg.NotNull(registrar, nameof(registrar));

            _facility.RegisterServices(registrar);

            foreach (ServiceRegistration registration in _registrar)
            {
                registrar.Register(registration);
            }
        }
    }

    public class FacilityBuilder<TFacility> : FacilityBuilder, IFacilityBuilder<TFacility>
        where TFacility : IFacility
    {
        private readonly List<IFacilityExtension<TFacility>> _extensions = new List<IFacilityExtension<TFacility>>();

        TFacility IFacilityBuilder<TFacility>.Facility => (TFacility) ((IFacilityBuilder)this).Facility;

        public FacilityBuilder(TFacility facility)
            : base(facility)
        {
        }

        public void UseExtension(IFacilityExtension<TFacility> extension)
        {
            Ensure.Arg.NotNull(extension, nameof(extension));

            _extensions.Add(extension);
        }

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
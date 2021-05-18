// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AppCore.DependencyInjection;

namespace AppCore.Hosting.Plugins
{
    public class TestComponentRegistry : IComponentRegistry, IEnumerable<ComponentRegistration>
    {
        private readonly List<ComponentRegistration> _registrations = new List<ComponentRegistration>();

        public IComponentRegistry Add(IEnumerable<ComponentRegistration> registrations)
        {
            foreach (ComponentRegistration registration in registrations)
            {
                _registrations.Add(registration);
            }

            return this;
        }

        public IComponentRegistry TryAdd(IEnumerable<ComponentRegistration> registrations)
        {
            foreach (ComponentRegistration registration in registrations)
            {
                if (_registrations.All(r => r.ContractType != registration.ContractType))
                    _registrations.Add(registration);
            }

            return this;
        }

        public IComponentRegistry TryAddEnumerable(IEnumerable<ComponentRegistration> registrations)
        {
            foreach (ComponentRegistration registration in registrations)
            {
                if (!_registrations.Any(
                    r => r.ContractType == registration.ContractType
                         && r.GetImplementationType() == registration.GetImplementationType()))
                {
                    _registrations.Add(registration);
                }
            }

            return this;
        }

        public IEnumerator<ComponentRegistration> GetEnumerator()
        {
            return _registrations.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
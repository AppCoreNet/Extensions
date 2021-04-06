// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using System.Collections.Generic;
using System.Linq;

namespace AppCore.DependencyInjection
{
    public class TestComponentRegistry : IComponentRegistry
    {
        private readonly List<ComponentRegistration> _registrations = new List<ComponentRegistration>();


        public IComponentRegistry Add(IEnumerable<ComponentRegistration> registrations)
        {
            _registrations.AddRange(registrations);
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
    }
}
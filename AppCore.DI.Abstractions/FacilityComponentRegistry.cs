// Licensed under the MIT License.
// Copyright (c) 2018 the AppCore .NET project.

using System;
using System.Collections.Generic;
using System.Linq;

namespace AppCore.DependencyInjection
{
    internal class FacilityComponentRegistry : IComponentRegistry
    {
        private readonly List<Func<IEnumerable<ComponentRegistration>>> _registrations =
            new List<Func<IEnumerable<ComponentRegistration>>>();

        public void RegisterCallback(Func<IEnumerable<ComponentRegistration>> registration)
        {
            _registrations.Add(registration);
        }

        public IEnumerable<ComponentRegistration> GetComponentRegistrations()
        {
            return _registrations.SelectMany(r => r());
        }
    }
}
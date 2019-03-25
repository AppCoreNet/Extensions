// Licensed under the MIT License.
// Copyright (c) 2018 the AppCore .NET project.

using System;
using System.Collections.Generic;
using System.Linq;

namespace AppCore.DependencyInjection
{
    public class TestComponentRegistry : IComponentRegistry
    {
        private readonly List<Func<IEnumerable<ComponentRegistration>>> _registrationCallbacks =
            new List<Func<IEnumerable<ComponentRegistration>>>();

        public void RegisterCallback(Func<IEnumerable<ComponentRegistration>> registration)
        {
            _registrationCallbacks.Add(registration);
        }

        public IEnumerable<ComponentRegistration> GetRegistrations()
        {
            return _registrationCallbacks.SelectMany(cb => cb())
                                         .ToList();
        }
    }
}
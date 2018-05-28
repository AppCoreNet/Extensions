// Licensed under the MIT License.
// Copyright (c) 2018 the AppCore .NET project.

using System;

namespace AppCore.DependencyInjection.Builder
{
    public class TypeRegistrationInfo : IComponentRegistrationInfoWithLifetime
    {
        private readonly Type _contractType;
        private readonly Type _componentType;

        public ComponentLifetime Lifetime { get; set; } = ComponentLifetime.Transient;

        public ComponentRegistrationFlags Flags { get; set; }

        internal TypeRegistrationInfo(Type contractType, Type componentType)
        {
            _contractType = contractType;
            _componentType = componentType;
        }

        internal ComponentRegistration CreateRegistration()
        {
            return ComponentRegistration.Create(_contractType, _componentType, Lifetime, Flags);
        }
    }
}
// Licensed under the MIT License.
// Copyright (c) 2018 the AppCore .NET project.

using System;
using System.Collections.Generic;

namespace AppCore.DependencyInjection.Builder
{
    public class AssemblyRegistrationInfo : IComponentRegistrationInfoWithLifetime
    {
        internal Type ContractType { get; } 

        public ComponentLifetime Lifetime { get; set; } = ComponentLifetime.Transient;

        public ComponentRegistrationFlags Flags { get; set; }

        public IList<Predicate<Type>> Filters { get; } = new List<Predicate<Type>>();

        internal AssemblyRegistrationInfo(Type contractType)
        {
            ContractType = contractType;
        }
    }
}
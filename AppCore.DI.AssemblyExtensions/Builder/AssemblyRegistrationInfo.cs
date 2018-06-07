// Licensed under the MIT License.
// Copyright (c) 2018 the AppCore .NET project.

using System;
using System.Collections.Generic;

namespace AppCore.DependencyInjection.Builder
{
    /// <summary>
    /// Provides component registration info for assembly scanner based registrations.
    /// </summary>
    public class AssemblyRegistrationInfo : IComponentRegistrationInfoWithLifetime
    {
        internal Type ContractType { get; }

        /// <inheritdoc />
        public ComponentLifetime Lifetime { get; set; } = ComponentLifetime.Transient;

        /// <inheritdoc />
        public ComponentRegistrationFlags Flags { get; set; }

        /// <summary>
        /// Gets the list of type filters.
        /// </summary>
        public IList<Predicate<Type>> Filters { get; } = new List<Predicate<Type>>();

        internal AssemblyRegistrationInfo(Type contractType)
        {
            ContractType = contractType;
        }
    }
}
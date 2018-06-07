// Licensed under the MIT License.
// Copyright (c) 2018 the AppCore .NET project.

using System;

namespace AppCore.DependencyInjection.Builder
{
    /// <summary>
    /// Provides component registration info for instance based registrations.
    /// </summary>
    public abstract class SingleInstanceRegistrationInfo : IComponentRegistrationInfo
    {
        internal Type ContractType { get; }

        /// <inheritdoc />
        public ComponentRegistrationFlags Flags { get; set; }

        internal SingleInstanceRegistrationInfo(Type contractType)
        {
            ContractType = contractType;
        }

        internal abstract ComponentRegistration CreateRegistration();
    }

    internal class SingleInstanceRegistrationInfo<TComponent> : SingleInstanceRegistrationInfo
    {
        private readonly TComponent _instance;

        public SingleInstanceRegistrationInfo(Type contractType, TComponent instance)
            : base(contractType)
        {
            _instance = instance;
        }

        internal override ComponentRegistration CreateRegistration()
        {
            return ComponentRegistration.Create(ContractType, _instance, Flags);
        }
    }
}
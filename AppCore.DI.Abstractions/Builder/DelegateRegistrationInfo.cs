// Licensed under the MIT License.
// Copyright (c) 2018 the AppCore .NET project.

using System;

namespace AppCore.DependencyInjection.Builder
{
    public abstract class DelegateRegistrationInfo : IComponentRegistrationInfoWithLifetime
    {
        internal Type ContractType { get; }

        public ComponentLifetime Lifetime { get; set; } = ComponentLifetime.Transient;

        public ComponentRegistrationFlags Flags { get; set; }

        internal DelegateRegistrationInfo(Type contractType)
        {
            ContractType = contractType;
        }

        internal abstract ComponentRegistration CreateRegistration();
    }

    internal class DelegateRegistrationInfo<TComponent> : DelegateRegistrationInfo
    {
        private readonly Func<IContainer, TComponent> _factory;

        public DelegateRegistrationInfo(Type contractType, Func<IContainer,  TComponent> factory)
            : base(contractType)
        {
            _factory = factory;
        }

        internal override ComponentRegistration CreateRegistration()
        {
            return ComponentRegistration.Create(ContractType, _factory, Lifetime, Flags);
        }
    }
}
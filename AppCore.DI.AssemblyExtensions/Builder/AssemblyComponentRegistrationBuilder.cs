// Licensed under the MIT License.
// Copyright (c) 2018 the AppCore .NET project.

using System;

namespace AppCore.DependencyInjection.Builder
{
    internal class AssemblyComponentRegistrationBuilder : IComponentRegistrationBuilder<AssemblyRegistrationInfo>
    {
        public IRegistrationBuilder Builder { get; }

        public Type ContractType => Builder.ContractType;

        public IComponentRegistry Registry => Builder.Registry;

        public AssemblyRegistrationInfo RegistrationInfo { get; }

        public AssemblyComponentRegistrationBuilder(
            IRegistrationBuilder builder,
            AssemblyRegistrationInfo registrationInfo)
        {
            Builder = builder;
            RegistrationInfo = registrationInfo;
        }

        public IComponentRegistrationBuilder<TypeRegistrationInfo> Add<TComponent>()
        {
            return Builder.Add<TComponent>();
        }

        public IComponentRegistrationBuilder<DelegateRegistrationInfo> Add<TComponent>(
            Func<IContainer, TComponent> factory)
        {
            return Builder.Add(factory);
        }

        public IComponentRegistrationBuilder<SingleInstanceRegistrationInfo> Add<TComponent>(TComponent instance)
        {
            return Builder.Add(instance);
        }

        public IComponentRegistrationBuilder<TypeRegistrationInfo> Add(Type componentType)
        {
            return Builder.Add(componentType);
        }
    }

    internal class AssemblyComponentRegistrationBuilder<TContract>
        : IComponentRegistrationBuilder<TContract, AssemblyRegistrationInfo>
    {
        public IRegistrationBuilder<TContract> Builder { get; }

        public IComponentRegistry Registry => Builder.Registry;

        public AssemblyRegistrationInfo RegistrationInfo { get; }

        public AssemblyComponentRegistrationBuilder(
            IRegistrationBuilder<TContract> builder,
            AssemblyRegistrationInfo registrationInfo)
        {
            Builder = builder;
            RegistrationInfo = registrationInfo;
        }

        public IComponentRegistrationBuilder<TContract, TypeRegistrationInfo> Add<TComponent>()
            where TComponent : TContract
        {
            return Builder.Add<TComponent>();
        }

        public IComponentRegistrationBuilder<TContract, DelegateRegistrationInfo> Add<TComponent>(
            Func<IContainer, TComponent> factory)
            where TComponent : TContract
        {
            return Builder.Add(factory);
        }

        public IComponentRegistrationBuilder<TContract, SingleInstanceRegistrationInfo> Add<TComponent>(
            TComponent instance)
            where TComponent : TContract
        {
            return Builder.Add(instance);
        }

        public IComponentRegistrationBuilder<TContract, TypeRegistrationInfo> Add(Type componentType)
        {
            return Builder.Add(componentType);
        }
    }
}
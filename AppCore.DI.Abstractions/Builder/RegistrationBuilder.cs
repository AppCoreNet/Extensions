// Licensed under the MIT License.
// Copyright (c) 2018 the AppCore .NET project.

using System;
using AppCore.Diagnostics;

namespace AppCore.DependencyInjection.Builder
{
    internal class RegistrationBuilder : IRegistrationBuilder
    {
        public IComponentRegistry Registry { get; }

        public Type ContractType { get; }

        public RegistrationBuilder(IComponentRegistry registry, Type contractType)
        {
            Registry = registry;
            ContractType = contractType;
        }

        public IComponentRegistrationBuilder<TypeRegistrationInfo> Add<TComponent>()
        {
            Ensure.Arg.OfType(typeof(TComponent), ContractType, nameof(TComponent));

            var registrationInfo = new TypeRegistrationInfo(ContractType, typeof(TComponent));
            Registry.RegisterCallback(() => new[] {registrationInfo.CreateRegistration()});

            return
                new ComponentRegistrationBuilder<TypeRegistrationInfo>(
                    this,
                    registrationInfo);
        }

        public IComponentRegistrationBuilder<DelegateRegistrationInfo> Add<TComponent>(
            Func<IContainer, TComponent> factory)
        {
            Ensure.Arg.NotNull(factory, nameof(factory));
            Ensure.Arg.OfType(typeof(TComponent), ContractType, nameof(factory));

            var registrationInfo = new DelegateRegistrationInfo<TComponent>(ContractType, factory);
            Registry.RegisterCallback(() => new[] {registrationInfo.CreateRegistration()});

            return
                new ComponentRegistrationBuilder<DelegateRegistrationInfo>(
                    this,
                    registrationInfo);
        }

        public IComponentRegistrationBuilder<SingleInstanceRegistrationInfo> Add<TComponent>(
            TComponent instance)
        {
            Ensure.Arg.NotNull(instance, nameof(instance));
            Ensure.Arg.OfType(typeof(TComponent), ContractType, nameof(instance));

            var registrationInfo = new SingleInstanceRegistrationInfo<TComponent>(ContractType, instance);
            Registry.RegisterCallback(() => new[] {registrationInfo.CreateRegistration()});

            return new ComponentRegistrationBuilder<SingleInstanceRegistrationInfo>(
                this,
                registrationInfo);
        }
        
        public IComponentRegistrationBuilder<TypeRegistrationInfo> Add(Type componentType)
        {
            Ensure.Arg.NotNull(componentType, nameof(componentType));
            Ensure.Arg.OfType(componentType, ContractType, nameof(componentType));

            var registrationInfo = new TypeRegistrationInfo(ContractType, componentType);
            Registry.RegisterCallback(() => new[] {registrationInfo.CreateRegistration()});

            return
                new ComponentRegistrationBuilder<TypeRegistrationInfo>(
                    this,
                    registrationInfo);
        }
    }

    internal class RegistrationBuilder<TContract> : IRegistrationBuilder<TContract>
    {
        public IComponentRegistry Registry { get; }

        public RegistrationBuilder(IComponentRegistry registry)
        {
            Registry = registry;
        }

        public IComponentRegistrationBuilder<TContract, TypeRegistrationInfo> Add<TComponent>()
            where TComponent : TContract
        {
            var registrationInfo = new TypeRegistrationInfo(typeof(TContract), typeof(TComponent));
            Registry.RegisterCallback(() => new[] {registrationInfo.CreateRegistration()});

            return
                new ComponentRegistrationBuilder<TContract, TypeRegistrationInfo>(
                    this,
                    registrationInfo);
        }

        public IComponentRegistrationBuilder<TContract, DelegateRegistrationInfo> Add<TComponent>(
            Func<IContainer, TComponent> factory)
            where TComponent : TContract
        {
            Ensure.Arg.NotNull(factory, nameof(factory));

            var registrationInfo = new DelegateRegistrationInfo<TComponent>(typeof(TContract), factory);
            Registry.RegisterCallback(() => new[] {registrationInfo.CreateRegistration()});

            return
                new ComponentRegistrationBuilder<TContract, DelegateRegistrationInfo>(
                    this,
                    registrationInfo);
        }

        public IComponentRegistrationBuilder<TContract, SingleInstanceRegistrationInfo> Add<TComponent>(
            TComponent instance)
            where TComponent : TContract
        {
            Ensure.Arg.NotNull(instance, nameof(instance));

            var registrationInfo =
                new SingleInstanceRegistrationInfo<TComponent>(typeof(TContract), instance);
            Registry.RegisterCallback(() => new[] {registrationInfo.CreateRegistration()});

            return new ComponentRegistrationBuilder<TContract, SingleInstanceRegistrationInfo>(
                this,
                registrationInfo);
        }

        public IComponentRegistrationBuilder<TContract, TypeRegistrationInfo> Add(Type componentType)
        {
            Ensure.Arg.NotNull(componentType, nameof(componentType));
            Ensure.Arg.OfType(componentType, typeof(TContract), nameof(componentType));

            var registrationInfo = new TypeRegistrationInfo(typeof(TContract), componentType);
            Registry.RegisterCallback(() => new[] {registrationInfo.CreateRegistration()});

            return
                new ComponentRegistrationBuilder<TContract, TypeRegistrationInfo>(
                    this,
                    registrationInfo);
        }
    }
}
// Licensed under the MIT License.
// Copyright (c) 2018 the AppCore .NET project.

using System;
using System.ComponentModel;

namespace AppCore.DependencyInjection.Builder
{
    public interface IRegistrationBuilder
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        IComponentRegistry Registry { get; }

        [EditorBrowsable(EditorBrowsableState.Never)]
        Type ContractType { get; }

        IComponentRegistrationBuilder<TypeRegistrationInfo> Add<TComponent>();

        IComponentRegistrationBuilder<DelegateRegistrationInfo> Add<TComponent>(
            Func<IContainer, TComponent> factory);

        IComponentRegistrationBuilder<SingleInstanceRegistrationInfo> Add<TComponent>(
            TComponent instance);

        IComponentRegistrationBuilder<TypeRegistrationInfo> Add(Type componentType);
    }

    public interface IRegistrationBuilder<TContract>
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        IComponentRegistry Registry { get; }

        IComponentRegistrationBuilder<TContract, TypeRegistrationInfo> Add<TComponent>()
            where TComponent : TContract;

        IComponentRegistrationBuilder<TContract, DelegateRegistrationInfo> Add<TComponent>(
            Func<IContainer, TComponent> factory)
            where TComponent : TContract;

        IComponentRegistrationBuilder<TContract, SingleInstanceRegistrationInfo> Add<TComponent>(
            TComponent instance)
            where TComponent : TContract;

        IComponentRegistrationBuilder<TContract, TypeRegistrationInfo> Add(Type componentType);
    }
}
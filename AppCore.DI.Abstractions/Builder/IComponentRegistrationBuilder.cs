// Licensed under the MIT License.
// Copyright (c) 2018 the AppCore .NET project.

using System.ComponentModel;

namespace AppCore.DependencyInjection.Builder
{
    public interface IComponentRegistrationBuilder<out TRegistrationInfo> : IRegistrationBuilder
        where TRegistrationInfo : IComponentRegistrationInfo
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        TRegistrationInfo RegistrationInfo { get; }
    }

    public interface IComponentRegistrationBuilder<TContract, out TRegistrationInfo>
        : IRegistrationBuilder<TContract>
        where TRegistrationInfo : IComponentRegistrationInfo
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        TRegistrationInfo RegistrationInfo { get; }
    }
}
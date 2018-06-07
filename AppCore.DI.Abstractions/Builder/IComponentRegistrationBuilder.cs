// Licensed under the MIT License.
// Copyright (c) 2018 the AppCore .NET project.

using System.ComponentModel;

namespace AppCore.DependencyInjection.Builder
{
    /// <summary>
    /// Represents a type to configure registered components.
    /// </summary>
    /// <typeparam name="TRegistrationInfo">The type of the registration info.</typeparam>
    public interface IComponentRegistrationBuilder<out TRegistrationInfo> : IRegistrationBuilder
        where TRegistrationInfo : IComponentRegistrationInfo
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        TRegistrationInfo RegistrationInfo { get; }
    }

    /// <summary>
    /// Represents a type to configure registered components.
    /// </summary>
    /// <typeparam name="TContract">The contract of the component.</typeparam>
    /// <typeparam name="TRegistrationInfo">Type type of the registration info.</typeparam>
    public interface IComponentRegistrationBuilder<TContract, out TRegistrationInfo>
        : IRegistrationBuilder<TContract>
        where TRegistrationInfo : IComponentRegistrationInfo
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        TRegistrationInfo RegistrationInfo { get; }
    }
}
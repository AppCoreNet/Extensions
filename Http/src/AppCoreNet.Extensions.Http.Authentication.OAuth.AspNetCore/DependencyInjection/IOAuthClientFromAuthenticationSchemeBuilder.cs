// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace
namespace AppCoreNet.Extensions.DependencyInjection;

/// <summary>
/// Builder object for OAuth client credentials authentication scheme from ASP.NET Core authentication
/// scheme.
/// </summary>
public interface IOAuthClientFromAuthenticationSchemeBuilder
{
    /// <summary>
    /// Gets the <see cref="IServiceCollection"/>.
    /// </summary>
    IServiceCollection Services { get; }

    /// <summary>
    /// Gets the name of the client authentication scheme.
    /// </summary>
    string Scheme { get; }
}
// Licensed under the MIT License.
// Copyright (c) 2018-2022 the AppCore .NET project.

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

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
// Licensed under the MIT License.
// Copyright (c) 2018-2022 the AppCore .NET project.

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Builder object for HTTP client authentication.
/// </summary>
public interface IHttpClientAuthenticationBuilder
{
    /// <summary>
    /// The underlying service collection.
    /// </summary>
    IServiceCollection Services { get; }
}
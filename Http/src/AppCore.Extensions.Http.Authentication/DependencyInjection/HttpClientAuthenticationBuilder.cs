// Licensed under the MIT License.
// Copyright (c) 2018-2022 the AppCore .NET project.

using AppCore.Diagnostics;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

internal sealed class HttpClientAuthenticationBuilder : IHttpClientAuthenticationBuilder
{
    public IServiceCollection Services { get; }

    public HttpClientAuthenticationBuilder(IServiceCollection services)
    {
        Ensure.Arg.NotNull(services);
        Services = services;
    }
}
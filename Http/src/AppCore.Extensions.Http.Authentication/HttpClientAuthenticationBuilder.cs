// Licensed under the MIT License.
// Copyright (c) 2018-2022 the AppCore .NET project.

using AppCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace AppCore.Extensions.Http.Authentication;

internal sealed class HttpClientAuthenticationBuilder : IHttpClientAuthenticationBuilder
{
    public IServiceCollection Services { get; }

    public HttpClientAuthenticationBuilder(IServiceCollection services)
    {
        Ensure.Arg.NotNull(services);
        Services = services;
    }
}
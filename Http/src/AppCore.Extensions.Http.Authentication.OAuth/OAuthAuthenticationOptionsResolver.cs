// Licensed under the MIT License.
// Copyright (c) 2018-2022 the AppCore .NET project.

using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace AppCore.Extensions.Http.Authentication.OAuth;

internal sealed class OAuthAuthenticationOptionsResolver<TOptions> : IOAuthAuthenticationOptionsResolver
    where TOptions : OAuthAuthenticationOptions
{
    private readonly IOptionsMonitor<TOptions> _options;

    public OAuthAuthenticationOptionsResolver(IOptionsMonitor<TOptions> options)
    {
        _options = options;
    }

    public Task<T?> TryGetOptionsAsync<T>(AuthenticationScheme scheme)
        where T : AuthenticationSchemeOptions
    {
        T? result = null;

        Type optionsType = typeof(T);
        if (typeof(TOptions) == optionsType && scheme.OptionsType == optionsType)
        {
            result = (T?)(object)_options.Get(scheme.Name);
        }

        return Task.FromResult(result);
    }
}
// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace AppCoreNet.Extensions.Http.Authentication.OAuth;

internal sealed class OAuthOptionsResolver<
    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] TOptions>
    : IOAuthOptionsResolver
    where TOptions : OAuthOptions
{
    private readonly IOptionsMonitor<TOptions> _options;

    public OAuthOptionsResolver(IOptionsMonitor<TOptions> options)
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
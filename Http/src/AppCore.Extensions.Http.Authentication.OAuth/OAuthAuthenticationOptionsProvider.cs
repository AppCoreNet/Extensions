// Licensed under the MIT License.
// Copyright (c) 2018-2022 the AppCore .NET project.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AppCore.Diagnostics;

namespace AppCore.Extensions.Http.Authentication.OAuth;

/// <summary>
/// Default implementation of the <see cref="IOAuthAuthenticationOptionsProvider"/>.
/// </summary>
public class OAuthAuthenticationOptionsProvider : IOAuthAuthenticationOptionsProvider
{
    private readonly IEnumerable<IOAuthAuthenticationOptionsResolver> _resolvers;

    /// <summary>
    /// Initializes a new instance of the <see cref="OAuthAuthenticationOptionsProvider"/> class.
    /// </summary>
    public OAuthAuthenticationOptionsProvider(IEnumerable<IOAuthAuthenticationOptionsResolver> resolvers)
    {
        Ensure.Arg.NotNull(resolvers);
        _resolvers = resolvers;
    }

    /// <inheritdoc />
    public virtual async Task<T> GetOptionsAsync<T>(AuthenticationScheme scheme)
        where T : AuthenticationSchemeOptions
    {
        Ensure.Arg.NotNull(scheme);

        T? result = null;
        foreach (IOAuthAuthenticationOptionsResolver resolver in _resolvers)
        {
            result = await resolver.TryGetOptionsAsync<T>(scheme);
            if (result != null)
                break;
        }

        if (result == null)
        {
            throw new InvalidOperationException(
                $"No options of type {typeof(T)} could be resolved for client authentication scheme {scheme.Name}.");
        }

        return result;
    }
}
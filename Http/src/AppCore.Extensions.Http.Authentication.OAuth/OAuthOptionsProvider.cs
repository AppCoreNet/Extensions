// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AppCoreNet.Diagnostics;

namespace AppCore.Extensions.Http.Authentication.OAuth;

/// <summary>
/// Default implementation of the <see cref="IOAuthOptionsProvider"/>.
/// </summary>
public class OAuthOptionsProvider : IOAuthOptionsProvider
{
    private readonly IEnumerable<IOAuthOptionsResolver> _resolvers;

    /// <summary>
    /// Initializes a new instance of the <see cref="OAuthOptionsProvider"/> class.
    /// </summary>
    /// <param name="resolvers">A collection of <see cref="IOAuthOptionsResolver"/>.</param>
    public OAuthOptionsProvider(IEnumerable<IOAuthOptionsResolver> resolvers)
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
        foreach (IOAuthOptionsResolver resolver in _resolvers)
        {
            result = await resolver.TryGetOptionsAsync<T>(scheme)
                                   .ConfigureAwait(false);

            if (result != null)
                break;
        }

        if (result == null)
        {
            throw new InvalidOperationException(
                $"No options of type '{typeof(T)}' could be resolved for client authentication scheme {scheme.Name}.");
        }

        return result;
    }
}
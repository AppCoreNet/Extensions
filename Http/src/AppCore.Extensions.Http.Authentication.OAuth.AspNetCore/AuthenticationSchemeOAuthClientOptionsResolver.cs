// Licensed under the MIT License.
// Copyright (c) 2018-2022 the AppCore .NET project.

using System;
using System.Threading.Tasks;
using AppCore.Diagnostics;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace AppCore.Extensions.Http.Authentication.OAuth.AspNetCore;

using IAuthenticationSchemeProvider = Microsoft.AspNetCore.Authentication.IAuthenticationSchemeProvider;

/// <summary>
/// Provides the base class for resolving OAuth client authentication options from ASP.NET Core authentication schemes.
/// </summary>
/// <typeparam name="TClientOptions">The type of the <see cref="AuthenticationSchemeOAuthClientOptions"/>.</typeparam>
/// <typeparam name="TOptions">The type of the <see cref="RemoteAuthenticationOptions"/>.</typeparam>
/// <typeparam name="THandler">The type of the <see cref="IAuthenticationHandler"/>.</typeparam>
public abstract class AuthenticationSchemeOAuthClientOptionsResolver<TClientOptions, TOptions, THandler> : IOAuthOptionsResolver
    where TClientOptions : AuthenticationSchemeOAuthClientOptions
    where TOptions : RemoteAuthenticationOptions
    where THandler : IAuthenticationHandler
{
    private readonly IAuthenticationSchemeProvider _authenticationSchemeProvider;
    private readonly IOptionsMonitor<TClientOptions> _clientOptions;
    private readonly IOptionsMonitor<TOptions> _authenticationSchemeOptions;

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthenticationSchemeOAuthClientOptionsResolver{TClientOptions,TOptions,THandler}"/> class.
    /// </summary>
    /// <param name="authenticationSchemeProvider">The <see cref="IAuthenticationSchemeProvider"/>.</param>
    /// <param name="authenticationSchemeOptions">The authentication scheme options.</param>
    /// <param name="clientOptions">The client authentication scheme options.</param>
    protected AuthenticationSchemeOAuthClientOptionsResolver(
        IAuthenticationSchemeProvider authenticationSchemeProvider,
        IOptionsMonitor<TOptions> authenticationSchemeOptions,
        IOptionsMonitor<TClientOptions> clientOptions)
    {
        Ensure.Arg.NotNull(authenticationSchemeProvider);
        Ensure.Arg.NotNull(authenticationSchemeOptions);
        Ensure.Arg.NotNull(clientOptions);

        _authenticationSchemeProvider = authenticationSchemeProvider;
        _clientOptions = clientOptions;
        _authenticationSchemeOptions = authenticationSchemeOptions;
    }

    /// <inheritdoc />
    public async Task<T?> TryGetOptionsAsync<T>(AuthenticationScheme scheme)
        where T : AuthenticationSchemeOptions
    {
        T? result = null;

        if (scheme.OptionsType == typeof(TClientOptions)
            && typeof(T) == typeof(OAuthClientOptions))
        {
            TClientOptions clientOptions = _clientOptions.Get(scheme.Name);

            Microsoft.AspNetCore.Authentication.AuthenticationScheme? authenticationScheme =
                string.IsNullOrWhiteSpace(clientOptions.Scheme)
                    ? await _authenticationSchemeProvider.GetDefaultChallengeSchemeAsync()
                                                         .ConfigureAwait(false)
                    : await _authenticationSchemeProvider.GetSchemeAsync(clientOptions.Scheme)
                                                         .ConfigureAwait(false);

            if (authenticationScheme is null)
            {
                throw new InvalidOperationException(
                    "No authentication scheme configured for getting client configuration. Either set the scheme name explicitly or set the default challenge scheme.");
            }

            if (authenticationScheme.HandlerType != typeof(THandler))
            {
                throw new InvalidOperationException(
                    "The authentication scheme handler configured for getting client configuration is not compatible.");

            }

            result = (T)(object)await GetOptionsFromSchemeAsync(
                    _clientOptions.Get(scheme.Name),
                    _authenticationSchemeOptions.Get(authenticationScheme.Name))
                .ConfigureAwait(false);
        }

        return result;
    }

    /// <summary>
    /// Must be implemented to resolve the <see cref="OAuthClientOptions"/> from the authentication scheme options.
    /// </summary>
    /// <param name="clientOptions">The <see cref="AuthenticationSchemeOAuthClientOptions"/>.</param>
    /// <param name="options">The <see cref="RemoteAuthenticationOptions"/>.</param>
    /// <returns></returns>
    protected abstract Task<OAuthClientOptions> GetOptionsFromSchemeAsync(TClientOptions clientOptions, TOptions options);
}
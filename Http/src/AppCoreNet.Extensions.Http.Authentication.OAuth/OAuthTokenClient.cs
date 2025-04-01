// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AppCoreNet.Diagnostics;
using IdentityModel.Client;

namespace AppCoreNet.Extensions.Http.Authentication.OAuth;

/// <summary>
/// Provides a client for OAuth token endpoints.
/// </summary>
public class OAuthTokenClient : IOAuthTokenClient
{
    private readonly HttpClient _client;
    private readonly IOAuthOptionsProvider _optionsProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="OAuthTokenClient"/> class.
    /// </summary>
    /// <param name="client">The underlying <see cref="HttpClient"/>.</param>
    /// <param name="optionsProvider">The options provider.</param>
    public OAuthTokenClient(HttpClient client, IOAuthOptionsProvider optionsProvider)
    {
        Ensure.Arg.NotNull(client);
        Ensure.Arg.NotNull(optionsProvider);

        _client = client;
        _optionsProvider = optionsProvider;
    }

    /// <inheritdoc />
    public async Task<TokenResponse> RequestClientAccessTokenAsync(
        AuthenticationScheme scheme,
        OAuthParameters? parameters = null,
        CancellationToken cancellationToken = default)
    {
        OAuthClientOptions options =
            await _optionsProvider.GetOptionsAsync<OAuthClientOptions>(scheme)
                                  .ConfigureAwait(false);

        using var request = new ClientCredentialsTokenRequest
        {
            RequestUri = options.TokenEndpoint,
            ClientId = options.ClientId!,
            ClientSecret = options.ClientSecret,
            ClientCredentialStyle = options.ClientCredentialStyle,
            Scope = options.Scope,
            Resource = new List<string>(options.Resource),
            Parameters = parameters?.Context ?? new Parameters(),
        };

        if (parameters != null && !string.IsNullOrWhiteSpace(parameters.Resource))
            request.Resource.Add(parameters.Resource);

        return await _client.RequestClientCredentialsTokenAsync(request, cancellationToken)
                            .ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<TokenResponse> RequestPasswordAccessTokenAsync(
        AuthenticationScheme scheme,
        OAuthParameters? parameters = null,
        CancellationToken cancellationToken = default)
    {
        OAuthPasswordOptions options =
            await _optionsProvider.GetOptionsAsync<OAuthPasswordOptions>(scheme)
                                  .ConfigureAwait(false);

        using var request = new PasswordTokenRequest
        {
            RequestUri = options.TokenEndpoint,
            ClientId = options.ClientId!,
            ClientSecret = options.ClientSecret,
            ClientCredentialStyle = options.ClientCredentialStyle,
            UserName = options.Username!,
            Password = options.Password,
            Scope = options.Scope,
            Resource = new List<string>(options.Resource),
            Parameters = parameters?.Context ?? new Parameters(),
        };

        if (parameters != null && !string.IsNullOrWhiteSpace(parameters.Resource))
            request.Resource.Add(parameters.Resource);

        return await _client.RequestPasswordTokenAsync(request, cancellationToken)
                            .ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<TokenResponse> RequestRefreshTokenAsync(
        AuthenticationScheme scheme,
        string refreshToken,
        OAuthParameters? parameters = null,
        CancellationToken cancellationToken = default)
    {
        Ensure.Arg.NotEmpty(refreshToken);

        OAuthClientOptions options =
            await _optionsProvider.GetOptionsAsync<OAuthClientOptions>(scheme)
                                  .ConfigureAwait(false);

        using var request = new RefreshTokenRequest
        {
            RequestUri = options.TokenEndpoint,
            ClientId = options.ClientId!,
            ClientSecret = options.ClientSecret,
            ClientCredentialStyle = options.ClientCredentialStyle,
            RefreshToken = refreshToken,
        };

        return await _client.RequestRefreshTokenAsync(request, cancellationToken)
                            .ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<TokenRevocationResponse> RevokeTokenAsync(
        AuthenticationScheme scheme,
        string token,
        string tokenTypeHint,
        CancellationToken cancellationToken = default)
    {
        Ensure.Arg.NotEmpty(token);
        Ensure.Arg.NotEmptyButNull(tokenTypeHint);

        OAuthClientOptions options =
            await _optionsProvider.GetOptionsAsync<OAuthClientOptions>(scheme)
                                  .ConfigureAwait(false);

        using var request = new TokenRevocationRequest
        {
            RequestUri = options.TokenEndpoint,
            ClientId = options.ClientId!,
            ClientSecret = options.ClientSecret,
            ClientCredentialStyle = options.ClientCredentialStyle,
            Token = token,
            TokenTypeHint = tokenTypeHint,
        };

        return await _client.RevokeTokenAsync(request, cancellationToken)
                            .ConfigureAwait(false);
    }
}
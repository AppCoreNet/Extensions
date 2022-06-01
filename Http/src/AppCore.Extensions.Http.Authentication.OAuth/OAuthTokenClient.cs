// Licensed under the MIT License.
// Copyright (c) 2018-2022 the AppCore .NET project.

using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AppCore.Diagnostics;
using IdentityModel.Client;

namespace AppCore.Extensions.Http.Authentication.OAuth;

/// <summary>
/// Provides a client for OAuth token endpoints.
/// </summary>
public class OAuthTokenClient : IOAuthTokenClient
{
    private readonly HttpClient _client;
    private readonly IOAuthAuthenticationOptionsProvider _optionsProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="OAuthTokenClient"/> class.
    /// </summary>
    /// <param name="client">The underlying <see cref="HttpClient"/>.</param>
    /// <param name="optionsProvider">The options provider.</param>
    public OAuthTokenClient(HttpClient client, IOAuthAuthenticationOptionsProvider optionsProvider)
    {
        Ensure.Arg.NotNull(client);
        Ensure.Arg.NotNull(optionsProvider);

        _client = client;
        _optionsProvider = optionsProvider;
    }

    /// <inheritdoc />
    public async Task<TokenResponse> RequestClientAccessToken(
        AuthenticationScheme scheme,
        OAuthAuthenticationParameters? parameters = null,
        CancellationToken cancellationToken = default)
    {
        var options = await _optionsProvider.GetOptionsAsync<OAuthClientAuthenticationOptions>(scheme);

        var request = new ClientCredentialsTokenRequest
        {
            RequestUri = options.TokenEndpoint,
            ClientId = options.ClientId,
            ClientSecret = options.ClientSecret,
            ClientCredentialStyle = options.ClientCredentialStyle,
            Scope = options.Scope,
            Resource = new List<string>(options.Resource),
            Parameters = parameters?.Context
        };

        if (parameters != null && !string.IsNullOrWhiteSpace(parameters.Resource))
            request.Resource.Add(parameters.Resource);

        return await _client.RequestClientCredentialsTokenAsync(request, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<TokenResponse> RequestPasswordAccessToken(
        AuthenticationScheme scheme,
        OAuthAuthenticationParameters? parameters = null,
        CancellationToken cancellationToken = default)
    {
        var options = await _optionsProvider.GetOptionsAsync<OAuthPasswordAuthenticationOptions>(scheme);

        var request = new PasswordTokenRequest
        {
            RequestUri = options.TokenEndpoint,
            ClientId = options.ClientId,
            ClientSecret = options.ClientSecret,
            ClientCredentialStyle = options.ClientCredentialStyle,
            UserName = options.Username,
            Password = options.Password,
            Scope = options.Scope,
            Resource = new List<string>(options.Resource),
            Parameters = parameters?.Context
        };

        if (parameters != null && !string.IsNullOrWhiteSpace(parameters.Resource))
            request.Resource.Add(parameters.Resource);

        return await _client.RequestPasswordTokenAsync(request, cancellationToken);
    }
}
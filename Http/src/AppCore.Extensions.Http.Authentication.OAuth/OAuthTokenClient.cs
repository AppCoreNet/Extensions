// Licensed under the MIT License.
// Copyright (c) 2018-2022 the AppCore .NET project.

using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AppCore.Diagnostics;
using IdentityModel.Client;
using Microsoft.Extensions.Options;

namespace AppCore.Extensions.Http.Authentication.OAuth;

/// <summary>
/// Provides a client for OAuth token endpoints.
/// </summary>
public class OAuthTokenClient : IOAuthTokenClient
{
    private readonly HttpClient _client;
    private readonly IOptionsMonitor<OAuthClientAuthenticationOptions> _clientOptions;
    private readonly IOptionsMonitor<OAuthPasswordAuthenticationOptions> _passwordOptions;

    /// <summary>
    /// Initializes a new instance of the <see cref="OAuthTokenClient"/> class.
    /// </summary>
    /// <param name="client">The underlying <see cref="HttpClient"/>.</param>
    /// <param name="clientOptions">The client authentication options.</param>
    /// <param name="passwordOptions">The password authentication options.</param>
    public OAuthTokenClient(
        HttpClient client,
        IOptionsMonitor<OAuthClientAuthenticationOptions> clientOptions,
        IOptionsMonitor<OAuthPasswordAuthenticationOptions> passwordOptions)
    {
        Ensure.Arg.NotNull(client);
        Ensure.Arg.NotNull(clientOptions);
        Ensure.Arg.NotNull(passwordOptions);

        _client = client;
        _clientOptions = clientOptions;
        _passwordOptions = passwordOptions;
    }

    /// <inheritdoc />
    public async Task<TokenResponse> RequestClientAccessToken(
        AuthenticationScheme scheme,
        OAuthAuthenticationParameters? parameters = null,
        CancellationToken cancellationToken = default)
    {
        scheme.EnsureClientScheme();
        OAuthClientAuthenticationOptions options = _clientOptions.Get(scheme.Name);

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
        scheme.EnsurePasswordScheme();
        OAuthPasswordAuthenticationOptions? options = _passwordOptions.Get(scheme.Name);

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
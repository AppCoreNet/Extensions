// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using AppCoreNet.Diagnostics;

namespace AppCoreNet.Extensions.Http.Authentication.OAuth;

/// <summary>
/// Provides a OAuth client authentication handler.
/// </summary>
public class OAuthClientHandler : IAuthenticationSchemeHandler<OAuthParameters>
{
    private readonly IOAuthTokenService _authTokenService;

    /// <summary>
    /// Initializes a new instance of the <see cref="OAuthClientHandler"/> class.
    /// </summary>
    /// <param name="authTokenService">The <see cref="IOAuthTokenService"/>.</param>
    public OAuthClientHandler(IOAuthTokenService authTokenService)
    {
        Ensure.Arg.NotNull(authTokenService);
        _authTokenService = authTokenService;
    }

    /// <inheritdoc />
    public async Task AuthenticateAsync(
        AuthenticationScheme scheme,
        HttpRequestMessage request,
        OAuthParameters? parameters,
        CancellationToken cancellationToken = default)
    {
        OAuthAccessToken accessToken =
            await _authTokenService.GetClientAccessTokenAsync(scheme, parameters, cancellationToken)
                                   .ConfigureAwait(false);

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken.AccessToken);
    }
}
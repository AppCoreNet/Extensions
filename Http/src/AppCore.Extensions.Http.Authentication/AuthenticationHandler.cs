// Licensed under the MIT License.
// Copyright (c) 2018-2022 the AppCore .NET project.

using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AppCore.Diagnostics;
using Microsoft.Extensions.Logging;

namespace AppCore.Extensions.Http.Authentication;

/// <summary>
/// Provides a <see cref="DelegatingHandler"/> for <see cref="HttpClient"/> which adds client
/// authentication to the request.
/// </summary>
/// <typeparam name="TParameters">The type of the <see cref="AuthenticationParameters"/>.</typeparam>
/// <typeparam name="THandler">The type of the <see cref="IAuthenticationSchemeHandler{TParameters}"/>.</typeparam>
public class AuthenticationHandler<TParameters, THandler> : DelegatingHandler
    where TParameters : AuthenticationParameters, new()
    where THandler : IAuthenticationSchemeHandler<TParameters>
{
    private readonly string _scheme;
    private readonly IAuthenticationSchemeProvider _schemes;
    private readonly IAuthenticationSchemeHandler<TParameters> _schemeHandler;
    private readonly TParameters? _parameters;
    private readonly ILogger _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthenticationHandler{TParameters,THandler}"/> class.
    /// </summary>
    /// <param name="scheme">The authentication scheme.</param>
    /// <param name="schemes">The available authentication schemes.</param>
    /// <param name="schemeHandler">The authentication scheme handler.</param>
    /// <param name="parameters">Additional authentication parameters.</param>
    /// <param name="logger"></param>
    public AuthenticationHandler(
        string scheme,
        IAuthenticationSchemeProvider schemes,
        THandler schemeHandler,
        TParameters? parameters,
        ILogger logger)
    {
        Ensure.Arg.NotNull(scheme);
        Ensure.Arg.NotNull(schemes);
        Ensure.Arg.NotNull(schemeHandler);
        Ensure.Arg.NotNull(logger);

        _scheme = scheme;
        _schemes = schemes;
        _schemeHandler = schemeHandler;
        _parameters = parameters;
        _logger = logger;
    }

    private async Task<AuthenticationScheme> GetSchemeAsync()
    {
        AuthenticationScheme? authenticationScheme = await _schemes.FindSchemeAsync(_scheme);
        if (authenticationScheme == null)
        {
            throw new InvalidOperationException($"There is no client authentication scheme registered with name {_scheme}.");
        }

        if (authenticationScheme.HandlerType != typeof(THandler))
        {
            throw new InvalidOperationException(
                $"There is no client authentication scheme registered with name {_scheme} and handler {typeof(THandler).Name}.");
        }

        return authenticationScheme;
    }

    /// <inheritdoc/>
    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        AuthenticationScheme scheme = await GetSchemeAsync();

        _logger.LogDebug("Authenticating HTTP request with scheme {schemeName}.", scheme.Name);

        await AuthenticateAsync(scheme, request, forceRenewal: false, cancellationToken);
        HttpResponseMessage response = await base.SendAsync(request, cancellationToken);

        // retry if 401
        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            _logger.LogDebug("Received 401 response, forcing renewal of authentication for scheme {schemeName}.", scheme.Name);

            response.Dispose();

            await AuthenticateAsync(scheme, request, forceRenewal: true, cancellationToken);
            return await base.SendAsync(request, cancellationToken);
        }

        return response;
    }

    /// <summary>
    /// Set an access token on the HTTP request
    /// </summary>
    /// <param name="scheme"></param>
    /// <param name="request"></param>
    /// <param name="forceRenewal"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    protected virtual async Task AuthenticateAsync(
        AuthenticationScheme scheme,
        HttpRequestMessage request,
        bool forceRenewal,
        CancellationToken cancellationToken)
    {
        TParameters? parameters = _parameters;
        if (parameters == null && forceRenewal)
        {
            parameters = new TParameters()
            {
                ForceRenewal = true
            };
        }

        if (parameters != null && parameters.ForceRenewal != forceRenewal)
        {
            parameters = parameters.Clone<TParameters>();
            parameters.ForceRenewal = forceRenewal;
        }

        await _schemeHandler.AuthenticateAsync(scheme, parameters, request, cancellationToken);
    }
}
// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;
using System.Collections.Generic;
using IdentityModel.Client;

namespace AppCoreNet.Extensions.Http.Authentication.OAuth;

/// <summary>
/// Provides the options for the OAuth authentication schemes.
/// </summary>
public abstract class OAuthOptions : AuthenticationSchemeOptions
{
    /// <summary>
    /// Gets or sets the URL of the OAuth token endpoint.
    /// </summary>
    public Uri? TokenEndpoint { get; set; }

    /// <summary>
    /// Gets or sets the URL of the OAuth token revocation endpoint.
    /// </summary>
    public Uri? TokenRevocationEndpoint { get; set; }

    /// <summary>
    /// Gets or sets the <c>client_id</c>.
    /// </summary>
    public string? ClientId { get; set; }

    /// <summary>
    /// Gets or sets the <c>client_secret</c>.
    /// </summary>
    public string? ClientSecret { get; set; }

    /// <summary>
    /// Gets or sets a value indicating how to transmit <see cref="ClientId"/> and <see cref="ClientSecret"/>.
    /// </summary>
    public ClientCredentialStyle ClientCredentialStyle { get; set; } = ClientCredentialStyle.PostBody;

    /// <summary>
    /// Gets or sets the authorization scope.
    /// </summary>
    public string? Scope { get; set; }

    /// <summary>
    /// Gets the resources to authenticate for.
    /// </summary>
    public ICollection<string> Resource { get; } = new HashSet<string>();
}
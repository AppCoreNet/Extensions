// Licensed under the MIT License.
// Copyright (c) 2018-2022 the AppCore .NET project.

using System;
using System.Collections.Generic;
using IdentityModel.Client;

namespace AppCore.Extensions.Http.Authentication.OAuth;

/// <summary>
/// Provides the options for the OAuth authentication schemes.
/// </summary>
public abstract class OAuthAuthenticationOptions : AuthenticationSchemeOptions
{
    /// <summary>
    /// Gets or sets the URL of the OAuth token endpoint.
    /// </summary>
    public Uri? TokenEndpoint { get; set; }

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
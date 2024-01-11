// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppCoreNet.Extensions.Http.Authentication;

/// <summary>
/// Represents all registered authentication schemes.
/// </summary>
public interface IAuthenticationSchemeProvider
{
    /// <summary>
    /// Adds a authentication scheme.
    /// </summary>
    /// <param name="scheme">The name of the scheme.</param>
    void AddScheme(AuthenticationScheme scheme);

    /// <summary>
    /// Gets all known authentication schemes.
    /// </summary>
    /// <returns>The collection of <see cref="AuthenticationScheme"/>.</returns>
    Task<IReadOnlyCollection<AuthenticationScheme>> GetAllSchemesAsync();

    /// <summary>
    /// Removes a authentication scheme.
    /// </summary>
    /// <param name="name">The name of the scheme.</param>
    void RemoveScheme(string name);

    /// <summary>
    /// Gets the authentication scheme with the specified name.
    /// </summary>
    /// <param name="name">The name of the scheme.</param>
    /// <returns>The scheme or <c>null</c> if not found.</returns>
    Task<AuthenticationScheme?> FindSchemeAsync(string name);
}
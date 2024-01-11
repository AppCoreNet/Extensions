// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System.Threading.Tasks;

namespace AppCore.Extensions.Http.Authentication.OAuth;

/// <summary>
/// Resolves the authentication options for OAuth schemes.
/// </summary>
public interface IOAuthOptionsResolver
{
    /// <summary>
    /// Tries to resolve the options for the specified <paramref name="scheme"/>.
    /// </summary>
    /// <param name="scheme">The <see cref="AuthenticationScheme"/> which options should be resolved.</param>
    /// <typeparam name="T">The type of the <see cref="AuthenticationSchemeOptions"/>.</typeparam>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task<T?> TryGetOptionsAsync<T>(AuthenticationScheme scheme)
        where T : AuthenticationSchemeOptions;
}
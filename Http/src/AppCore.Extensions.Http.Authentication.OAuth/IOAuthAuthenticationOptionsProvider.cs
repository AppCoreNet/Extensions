// Licensed under the MIT License.
// Copyright (c) 2018-2022 the AppCore .NET project.

using System.Threading.Tasks;

namespace AppCore.Extensions.Http.Authentication.OAuth;

/// <summary>
/// Resolves <see cref="OAuthAuthenticationOptions"/> for OAuth schemes.
/// </summary>
public interface IOAuthAuthenticationOptionsProvider
{
    /// <summary>
    /// Resolves the options for the specified <paramref name="scheme"/>.
    /// </summary>
    /// <param name="scheme">The <see cref="AuthenticationScheme"/> which options should be resolved.</param>
    /// <typeparam name="T">The type of the <see cref="OAuthAuthenticationOptions"/>.</typeparam>
    /// <returns></returns>
    Task<T> GetOptionsAsync<T>(AuthenticationScheme scheme)
        where T : OAuthAuthenticationOptions;
}
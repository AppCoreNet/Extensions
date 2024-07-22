// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;

namespace AppCoreNet.Extensions.Http.Authentication;

/// <summary>
/// Provides the base class for authentication scheme options.
/// </summary>
public abstract class AuthenticationSchemeOptions
{
    /// <summary>
    /// Gets or sets the <see cref="System.TimeProvider"/> used for testing.
    /// </summary>
    public TimeProvider? TimeProvider { get; set; }
}
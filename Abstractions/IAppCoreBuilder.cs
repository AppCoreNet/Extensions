// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using Microsoft.Extensions.DependencyInjection;

namespace AppCore.DependencyInjection
{
    /// <summary>
    /// Represents the builder for AppCore services.
    /// </summary>
    public interface IAppCoreBuilder
    {
        /// <summary>
        /// Gets the <see cref="IServiceCollection"/>.
        /// </summary>
        IServiceCollection Services { get; }
    }
}

// Licensed under the MIT License.
// Copyright (c) 2018 the AppCore .NET project.

namespace AppCore.DependencyInjection.Builder
{
    /// <summary>
    /// Represents a type which provides component registration information.
    /// </summary>
    public interface IComponentRegistrationInfoWithLifetime : IComponentRegistrationInfo
    {
        /// <summary>
        /// Gets or sets the component lifetime.
        /// </summary>
        ComponentLifetime Lifetime { get; set; }
    }
}
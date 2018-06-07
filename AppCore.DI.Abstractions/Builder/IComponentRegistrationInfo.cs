// Licensed under the MIT License.
// Copyright (c) 2018 the AppCore .NET project.

namespace AppCore.DependencyInjection.Builder
{
    /// <summary>
    /// Represents a type which provides component registration information.
    /// </summary>
    public interface IComponentRegistrationInfo
    {
        /// <summary>
        /// Gets or sets the registration flags.
        /// </summary>
        ComponentRegistrationFlags Flags { get; set; }
    }
}
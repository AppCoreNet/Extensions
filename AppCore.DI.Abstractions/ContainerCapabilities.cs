// Licensed under the MIT License.
// Copyright (c) 2018 the AppCore .NET project.

using System;

namespace AppCore.DependencyInjection
{
    /// <summary>
    /// Defines container capibilities.
    /// </summary>
    [Flags]
    public enum ContainerCapabilities
    {
        /// <summary>
        /// No special capbilities.
        /// </summary>
        None,

        /// <summary>
        /// The container supports contra variance.
        /// </summary>
        ContraVariance = 0x00000001,
    }
}
// Licensed under the MIT License.
// Copyright (c) 2018 the AppCore .NET project.

using System;

namespace AppCore.DependencyInjection
{
    /// <summary>
    /// Defines component registration options.
    /// </summary>
    [Flags]
    public enum ComponentRegistrationFlags
    {
        /// <summary>
        /// No special registration options.
        /// </summary>
        None,

        /// <summary>
        /// Skips registration if component with same contract is already registered.
        /// </summary>
        IfNoneRegistered = 0x01,

        /// <summary>
        /// Skips registration if component with same contract and implementation is already registered.
        /// </summary>
        IfNotRegistered = 0x03
    }
}
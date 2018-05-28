// Licensed under the MIT License.
// Copyright (c) 2018 the AppCore .NET project.

using System;

namespace AppCore.DependencyInjection
{
    /// <summary>
    /// Represents a scope of a <see cref="IContainer"/>.
    /// </summary>
    /// <remarks>
    /// All resolved components with <see cref="ComponentLifetime.Scoped"/> will get
    /// disposed when the scope is disposed.
    /// </remarks>
    public interface IContainerScope : IDisposable
    {
        /// <summary>
        /// Gets the scoped <see cref="IContainer"/>.
        /// </summary>
        IContainer Container { get; }
    }
}
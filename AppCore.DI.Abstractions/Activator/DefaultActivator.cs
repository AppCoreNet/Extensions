// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using System;
using AppCore.Diagnostics;

namespace AppCore.DependencyInjection.Activator
{
    /// <summary>
    /// Represents the default activator.
    /// </summary>
    public sealed class DefaultActivator : IActivator
    {
        /// <summary>
        /// Gets the static instance of the <see cref="DefaultActivator"/>.
        /// </summary>
        public static DefaultActivator Instance { get; } = new();

        /// <inheritdoc />
        public object CreateInstance(Type instanceType, params object[] parameters)
        {
            Ensure.Arg.NotNull(instanceType, nameof(instanceType));
            return System.Activator.CreateInstance(instanceType, parameters);
        }

        /// <inheritdoc />
        public object ResolveOrCreateInstance(Type type)
        {
            return CreateInstance(type);
        }
    }
}
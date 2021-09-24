// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using System;
using AppCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace
namespace AppCore.DependencyInjection
{
    /// <summary>
    /// Provides extension methods to register plugins.
    /// </summary>
    public static class PluginAppCoreBuilderExtensions
    {
        /// <summary>
        /// Adds plugins.
        /// </summary>
        /// <param name="builder">The <see cref="IAppCoreBuilder"/>.</param>
        /// <param name="configure">The configuration delegate.</param>
        /// <returns>The passed <see cref="IAppCoreBuilder"/> to allow chaining.</returns>
        public static IAppCoreBuilder AddPlugins(
            this IAppCoreBuilder builder,
            Action<PluginFacility> configure = null)
        {
            Ensure.Arg.NotNull(builder, nameof(builder));
            builder.Services.AddFacility(configure);
            return builder;
        }
    }
}
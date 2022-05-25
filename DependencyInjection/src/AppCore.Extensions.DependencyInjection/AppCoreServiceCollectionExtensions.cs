// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using Microsoft.Extensions.DependencyInjection;

namespace AppCore.DependencyInjection
{
    /// <summary>
    /// Provides extension methods for the <see cref="IServiceCollection"/> interface.
    /// </summary>
    public static class AppCoreServiceCollectionExtensions
    {
        /// <summary>
        /// Adds AppCore services.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <returns>The <see cref="IAppCoreBuilder"/>.</returns>
        public static IAppCoreBuilder AddAppCore(this IServiceCollection services)
        {
            return new AppCoreBuilder(services);
        }
    }
}
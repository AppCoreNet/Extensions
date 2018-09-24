// Licensed under the MIT License.
// Copyright (c) 2018 the AppCore .NET project.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AppCore.Diagnostics;
using Microsoft.Extensions.DependencyModel;

namespace AppCore.DependencyInjection.Builder
{
    /// <summary>
    /// Provides extension methods for <see cref="IRegistrationBuilder"/> and <see cref="IRegistrationBuilder{TContract}"/>.
    /// </summary>
    public static class DependencyContextRegistrationBuilderExtensions
    {
        private static IEnumerable<Assembly> LoadAssemblies(DependencyContext dependencyContext)
        {
            return dependencyContext.GetDefaultAssemblyNames()
                                    .Select(Assembly.Load);
        }

        /// <summary>
        /// Adds components by scanning all assemblies of the <see cref="DependencyContext"/>.
        /// </summary>
        /// <param name="builder">The <see cref="IComponentRegistrationBuilder{TRegistrationInfo}"/>.</param>
        /// <param name="dependencyContext">The <see cref="DependencyContext"/> which should be scanned.</param>
        /// <returns>The <see cref="IComponentRegistrationBuilder{TRegistrationInfo}"/>.</returns>
        /// <exception cref="ArgumentNullException">Argument <paramref name="builder"/> is <c>null</c>. </exception>
        public static IComponentRegistrationBuilder<AssemblyRegistrationInfo> AddFromDependencyContext(
            this IRegistrationBuilder builder,
            DependencyContext dependencyContext
            #if NET452 || NETSTANDARD1_6
                = null
            #endif
            )
        {
            Ensure.Arg.NotNull(builder, nameof(builder));

#if NET452 || NETSTANDARD1_6
            dependencyContext = dependencyContext ?? DependencyContext.Default;
 #endif

            return builder.AddFromAssemblies(LoadAssemblies(dependencyContext));
        }

        /// <summary>
        /// Adds components by scanning all assemblies of the <see cref="DependencyContext"/>.
        /// </summary>
        /// <param name="builder">The <see cref="IComponentRegistrationBuilder{TContract, TRegistrationInfo}"/>.</param>
        /// <param name="dependencyContext">The <see cref="DependencyContext"/> which should be scanned.</param>
        /// <returns>The <see cref="IComponentRegistrationBuilder{TContract, TRegistrationInfo}"/>.</returns>
        /// <exception cref="ArgumentNullException">Argument <paramref name="builder"/> is <c>null</c>. </exception>
        public static IComponentRegistrationBuilder<TContract, AssemblyRegistrationInfo> AddFromDependencyContext<TContract>(
            this IRegistrationBuilder<TContract> builder,
            DependencyContext dependencyContext
            #if NET452 || NETSTANDARD1_6
                = null
            #endif
            )
        {
            Ensure.Arg.NotNull(builder, nameof(builder));

#if NET452 || NETSTANDARD1_6
            dependencyContext = dependencyContext ?? DependencyContext.Default;
#endif

            return builder.AddFromAssemblies(LoadAssemblies(dependencyContext));
        }
    }
}

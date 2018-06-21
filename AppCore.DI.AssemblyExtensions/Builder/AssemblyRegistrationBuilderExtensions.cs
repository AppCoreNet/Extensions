// Licensed under the MIT License.
// Copyright (c) 2018 the AppCore .NET project.

using System;
using System.Reflection;
using AppCore.Diagnostics;

#if NETSTANDARD1_6
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyModel;
#endif

namespace AppCore.DependencyInjection.Builder
{
    /// <summary>
    /// Provides extension methods for <see cref="IRegistrationBuilder"/> and <see cref="IRegistrationBuilder{TContract}"/>.
    /// </summary>
    public static class AssemblyRegistrationBuilderExtensions
    {
        private static void RegisterScannerCallback(
            this IRegistrationBuilder builder,
            AssemblyScanner scanner,
            AssemblyRegistrationInfo registrationInfo)
        {
            builder.Registry.RegisterCallback(() =>
            {
                foreach (Predicate<Type> registrationFilter in registrationInfo.Filters)
                {
                    scanner.Filters.Add(registrationFilter);
                }

                return scanner.CreateRegistrations(registrationInfo);
            });
        }

        private static void RegisterScannerCallback<TContract>(
            this IRegistrationBuilder<TContract> builder,
            AssemblyScanner scanner,
            AssemblyRegistrationInfo registrationInfo)
        {
            builder.Registry.RegisterCallback(() =>
            {
                foreach (Predicate<Type> registrationFilter in registrationInfo.Filters)
                {
                    scanner.Filters.Add(registrationFilter);
                }

                return scanner.CreateRegistrations(registrationInfo);
            });
        }

        /// <summary>
        /// Adds components by scanning the specified <see cref="Assembly"/>.
        /// </summary>
        /// <param name="builder">The <see cref="IComponentRegistrationBuilder{TRegistrationInfo}"/>.</param>
        /// <param name="assembly">The <see cref="Assembly"/> which should be scanned.</param>
        /// <returns>The <see cref="IComponentRegistrationBuilder{TRegistrationInfo}"/>.</returns>
        /// <exception cref="ArgumentNullException">
        ///     Argument <paramref name="builder"/> or <paramref name="assembly"/> is <c>null</c>.
        /// </exception>
        public static IComponentRegistrationBuilder<AssemblyRegistrationInfo> AddFromAssembly(
            this IRegistrationBuilder builder,
            Assembly assembly)
        {
            Ensure.Arg.NotNull(builder, nameof(builder));
            Ensure.Arg.NotNull(assembly, nameof(assembly));

            var registrationInfo = new AssemblyRegistrationInfo(builder.ContractType)
            {
                Lifetime = builder.DefaultLifetime
            };

            var scanner = new AssemblyScanner(builder.ContractType);
            scanner.Assemblies.Add(assembly);
            builder.RegisterScannerCallback(scanner, registrationInfo);

            return new AssemblyComponentRegistrationBuilder(builder, registrationInfo);
        }

        /// <summary>
        /// Adds components by scanning the specified <see cref="Assembly"/>.
        /// </summary>
        /// <param name="builder">The <see cref="IComponentRegistrationBuilder{TContract, TRegistrationInfo}"/>.</param>
        /// <param name="assembly">The <see cref="Assembly"/> which should be scanned.</param>
        /// <returns>The <see cref="IComponentRegistrationBuilder{TContract, TRegistrationInfo}"/>.</returns>
        /// <exception cref="ArgumentNullException">
        ///     Argument <paramref name="builder"/> or <paramref name="assembly"/> is <c>null</c>.
        /// </exception>
        public static IComponentRegistrationBuilder<TContract, AssemblyRegistrationInfo> AddFromAssembly<TContract>(
            this IRegistrationBuilder<TContract> builder,
            Assembly assembly)
        {
            Ensure.Arg.NotNull(builder, nameof(builder));
            Ensure.Arg.NotNull(assembly, nameof(assembly));

            var registrationInfo = new AssemblyRegistrationInfo(typeof(TContract))
            {
                Lifetime = builder.DefaultLifetime
            };

            var scanner = new AssemblyScanner(typeof(TContract));
            scanner.Assemblies.Add(assembly);
            builder.RegisterScannerCallback(scanner, registrationInfo);

            return new AssemblyComponentRegistrationBuilder<TContract>(builder, registrationInfo);
        }

#if NETSTANDARD1_6

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
            DependencyContext dependencyContext = null)
        {
            Ensure.Arg.NotNull(builder, nameof(builder));

            dependencyContext = dependencyContext ?? DependencyContext.Default;

            var registrationInfo = new AssemblyRegistrationInfo(builder.ContractType)
            {
                Lifetime = builder.DefaultLifetime
            };

            var scanner = new AssemblyScanner(builder.ContractType, LoadAssemblies(dependencyContext));
            builder.RegisterScannerCallback(scanner, registrationInfo);

            return new AssemblyComponentRegistrationBuilder(builder, registrationInfo);
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
            DependencyContext dependencyContext = null)
        {
            Ensure.Arg.NotNull(builder, nameof(builder));

            dependencyContext = dependencyContext ?? DependencyContext.Default;

            var registrationInfo = new AssemblyRegistrationInfo(typeof(TContract))
            {
                Lifetime = builder.DefaultLifetime
            };

            var scanner = new AssemblyScanner(typeof(TContract), LoadAssemblies(dependencyContext));
            builder.RegisterScannerCallback(scanner, registrationInfo);

            return new AssemblyComponentRegistrationBuilder<TContract>(builder, registrationInfo);
        }

#endif
    }
}

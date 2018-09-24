// Licensed under the MIT License.
// Copyright (c) 2018 the AppCore .NET project.

using System;
using System.Collections.Generic;
using System.Reflection;
using AppCore.Diagnostics;

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
        /// Adds components by scanning the specified <see cref="Assembly"/>'s.
        /// </summary>
        /// <param name="builder">The <see cref="IComponentRegistrationBuilder{TRegistrationInfo}"/>.</param>
        /// <param name="assemblies">The <see cref="Assembly"/>'s which should be scanned.</param>
        /// <returns>The <see cref="IComponentRegistrationBuilder{TRegistrationInfo}"/>.</returns>
        /// <exception cref="ArgumentNullException">
        ///     Argument <paramref name="builder"/> or <paramref name="assemblies"/> is <c>null</c>.
        /// </exception>
        public static IComponentRegistrationBuilder<AssemblyRegistrationInfo> AddFromAssemblies(
            this IRegistrationBuilder builder,
            IEnumerable<Assembly> assemblies)
        {
            Ensure.Arg.NotNull(builder, nameof(builder));
            Ensure.Arg.NotNull(assemblies, nameof(assemblies));

            var registrationInfo = new AssemblyRegistrationInfo(builder.ContractType)
            {
                Lifetime = builder.DefaultLifetime
            };

            var scanner = new AssemblyScanner(builder.ContractType);
            foreach (Assembly assembly in assemblies)
            {
                scanner.Assemblies.Add(assembly);
            }

            builder.RegisterScannerCallback(scanner, registrationInfo);

            return new AssemblyComponentRegistrationBuilder(builder, registrationInfo);
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
            Ensure.Arg.NotNull(assembly, nameof(assembly));
            return builder.AddFromAssemblies(new[] {assembly});
        }

        /// <summary>
        /// Adds components by scanning the specified <see cref="Assembly"/>'s.
        /// </summary>
        /// <param name="builder">The <see cref="IComponentRegistrationBuilder{TContract, TRegistrationInfo}"/>.</param>
        /// <param name="assemblies">The <see cref="Assembly"/>'s which should be scanned.</param>
        /// <returns>The <see cref="IComponentRegistrationBuilder{TContract, TRegistrationInfo}"/>.</returns>
        /// <exception cref="ArgumentNullException">
        ///     Argument <paramref name="builder"/> or <paramref name="assemblies"/> is <c>null</c>.
        /// </exception>
        public static IComponentRegistrationBuilder<TContract, AssemblyRegistrationInfo> AddFromAssemblies<TContract>(
            this IRegistrationBuilder<TContract> builder,
            IEnumerable<Assembly> assemblies)
        {
            Ensure.Arg.NotNull(builder, nameof(builder));
            Ensure.Arg.NotNull(assemblies, nameof(assemblies));

            var registrationInfo = new AssemblyRegistrationInfo(typeof(TContract))
            {
                Lifetime = builder.DefaultLifetime
            };

            var scanner = new AssemblyScanner(typeof(TContract));
            foreach (Assembly assembly in assemblies)
            {
                scanner.Assemblies.Add(assembly);
            }

            builder.RegisterScannerCallback(scanner, registrationInfo);

            return new AssemblyComponentRegistrationBuilder<TContract>(builder, registrationInfo);
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
            Ensure.Arg.NotNull(assembly, nameof(assembly));
            return builder.AddFromAssemblies(new[] {assembly});
        }
    }
}

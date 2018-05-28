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

        public static IComponentRegistrationBuilder<AssemblyRegistrationInfo> AddFromAssembly(
            this IRegistrationBuilder builder,
            Assembly assembly)
        {
            Ensure.Arg.NotNull(builder, nameof(builder));
            Ensure.Arg.NotNull(assembly, nameof(assembly));

            var registrationInfo = new AssemblyRegistrationInfo(builder.ContractType);
            var scanner = new AssemblyScanner(builder.ContractType);
            scanner.Assemblies.Add(assembly);
            builder.RegisterScannerCallback(scanner, registrationInfo);

            return new AssemblyComponentRegistrationBuilder(builder, registrationInfo);
        }

        public static IComponentRegistrationBuilder<TContract, AssemblyRegistrationInfo> AddFromAssembly<TContract>(
            this IRegistrationBuilder<TContract> builder,
            Assembly assembly)
        {
            Ensure.Arg.NotNull(builder, nameof(builder));
            Ensure.Arg.NotNull(assembly, nameof(assembly));

            var registrationInfo = new AssemblyRegistrationInfo(typeof(TContract));
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

        public static IComponentRegistrationBuilder<AssemblyRegistrationInfo> AddFromAllAssemblies(
            this IRegistrationBuilder builder,
            DependencyContext dependencyContext = null)
        {
            Ensure.Arg.NotNull(builder, nameof(builder));

            dependencyContext = dependencyContext ?? DependencyContext.Default;

            var registrationInfo = new AssemblyRegistrationInfo(builder.ContractType);
            var scanner = new AssemblyScanner(builder.ContractType, LoadAssemblies(dependencyContext));
            builder.RegisterScannerCallback(scanner, registrationInfo);

            return new AssemblyComponentRegistrationBuilder(builder, registrationInfo);
        }

        public static IComponentRegistrationBuilder<TContract, AssemblyRegistrationInfo> AddFromAllAssemblies<TContract>(
            this IRegistrationBuilder<TContract> builder,
            DependencyContext dependencyContext = null)
        {
            Ensure.Arg.NotNull(builder, nameof(builder));

            dependencyContext = dependencyContext ?? DependencyContext.Default;

            var registrationInfo = new AssemblyRegistrationInfo(typeof(TContract));
            var scanner = new AssemblyScanner(typeof(TContract), LoadAssemblies(dependencyContext));
            builder.RegisterScannerCallback(scanner, registrationInfo);

            return new AssemblyComponentRegistrationBuilder<TContract>(builder, registrationInfo);
        }

#endif

#if NETSTANDARD2_0

        public static IComponentRegistrationBuilder<AssemblyRegistrationInfo> AddFromAllAssemblies(
            this IRegistrationBuilder builder)
        {
            Ensure.Arg.NotNull(builder, nameof(builder));

            var registrationInfo = new AssemblyRegistrationInfo(builder.ContractType);
            var scanner = new AssemblyScanner(builder.ContractType, AppDomain.CurrentDomain.GetAssemblies());
            builder.RegisterScannerCallback(scanner, registrationInfo);

            return new AssemblyComponentRegistrationBuilder(builder, registrationInfo);
        }

        public static IComponentRegistrationBuilder<TContract, AssemblyRegistrationInfo> AddFromAllAssemblies<TContract>(
            this IRegistrationBuilder<TContract> builder)
        {
            Ensure.Arg.NotNull(builder, nameof(builder));

            var registrationInfo = new AssemblyRegistrationInfo(typeof(TContract));
            var scanner = new AssemblyScanner(typeof(TContract), AppDomain.CurrentDomain.GetAssemblies());
            builder.RegisterScannerCallback(scanner, registrationInfo);

            return new AssemblyComponentRegistrationBuilder<TContract>(builder, registrationInfo);
        }

#endif
    }
}

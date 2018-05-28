// Licensed under the MIT License.
// Copyright (c) 2018 the AppCore .NET project.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AppCore.Diagnostics;

namespace AppCore.DependencyInjection
{
    public class AssemblyScanner
    {
        private static readonly List<string> DefaultExcludedAssemblies = new List<string>
        {
            "System",
            "Microsoft",
            "AppCore"
        };

        public Type ContractType { get; }

        public IList<Assembly> Assemblies { get; }

        public IList<Predicate<Type>> Filters { get; } = new List<Predicate<Type>>();

        public AssemblyScanner(Type contractType, IEnumerable<Assembly> assemblies)
        {
            Ensure.Arg.NotNull(contractType, nameof(contractType));
            Ensure.Arg.NotNull(assemblies, nameof(assemblies));

            ContractType = contractType;
            Assemblies = new List<Assembly>(assemblies);
            Filters.Add(FilterSystemAssemblies);
        }

        public AssemblyScanner(Type contractType)
            : this(contractType, Enumerable.Empty<Assembly>())
        {
        }

        private bool FilterSystemAssemblies(Type type)
        {
            string assemblyName = type.GetTypeInfo().Assembly.GetName().Name;
            return DefaultExcludedAssemblies.All(a => !assemblyName.StartsWith(a));
        }

        private IEnumerable<Type> GetTypes(Assembly assembly)
        {
            IEnumerable<Type> exportedTypes = assembly.ExportedTypes.Where(
                t =>
                {
                    TypeInfo ti = t.GetTypeInfo();
                    return ti.IsClass
                           && !ti.IsAbstract
                           && ti.DeclaredConstructors.Any(
                               ci => ci.IsPublic && !ci
                                         .IsStatic);
                });

            exportedTypes = exportedTypes.Where(
                t =>
                {
                    IEnumerable<Type> assignableTypes = t.GetTypesAssignableFrom().ToArray();

                    assignableTypes = assignableTypes.Concat(
                        assignableTypes.Where(
                                           t2 => t2.IsConstructedGenericType)
                                       .Select(t2 => t2.GetGenericTypeDefinition()));

                    return assignableTypes
                        .Contains(ContractType);
                });

            foreach (Predicate<Type> registrationFilter in Filters)
            {
                exportedTypes = exportedTypes.Where(et => registrationFilter(et));
            }

            return exportedTypes;
        }

        public IEnumerable<Type> ScanAssemblies()
        {
            return Assemblies.SelectMany(GetTypes);
        }
    }
}


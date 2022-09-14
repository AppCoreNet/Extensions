// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AppCore.Diagnostics;

namespace AppCore.Extensions.DependencyInjection;

/// <summary>
/// Represents a type used to scan assemblies for exported types.
/// </summary>
public class AssemblyScanner
{
    private static readonly List<string> DefaultExcludedAssemblies = new List<string>
    {
        "System",
        "Microsoft",
        "AppCore"
    };

    /// <summary>
    /// The contract for which types are scanned.
    /// </summary>
    public Type ContractType { get; }

    /// <summary>
    /// Gets or sets a value whether to search for private/internal types.
    /// </summary>
    public bool IncludePrivateTypes { get; set; }

    /// <summary>
    /// Gets the <see cref="IList{T}"/> of <see cref="Assembly"/> to scan.
    /// </summary>
    public IList<Assembly> Assemblies { get; }

    /// <summary>
    /// Gets the <see cref="IList{T}"/> of type filters.
    /// </summary>
    public IList<Predicate<Type>> Filters { get; } = new List<Predicate<Type>>();

    /// <summary>
    /// Initializes a new instance of the <see cref="AssemblyScanner"/> class.
    /// </summary>
    /// <param name="contractType">The type being implemented by types to search for.</param>
    /// <param name="assemblies">The list of assemblies to scan.</param>
    /// <exception cref="ArgumentNullException">
    ///     Argument <paramref name="contractType"/> or <paramref name="assemblies"/> is <c>null</c>.
    /// </exception>
    public AssemblyScanner(Type contractType, IEnumerable<Assembly> assemblies)
    {
        Ensure.Arg.NotNull(contractType);
        Ensure.Arg.NotNull(assemblies);

        ContractType = contractType;
        Assemblies = new List<Assembly>(assemblies);
        Filters.Add(FilterSystemAssemblies);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AssemblyScanner"/> class.
    /// </summary>
    /// <param name="contractType">The type being implemented by types to search for.</param>
    /// <exception cref="ArgumentNullException">Argument <paramref name="contractType"/> is <c>null</c>.</exception>
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
        IEnumerable<Type> exportedTypes = IncludePrivateTypes
            ? assembly.GetTypes()
            : assembly.GetExportedTypes();

        exportedTypes = exportedTypes.Where(
            t =>
            {
                TypeInfo ti = t.GetTypeInfo();
                return ti.IsClass
                       && !ti.IsAbstract
                       && ti.DeclaredConstructors.Any(ci => ci.IsPublic && !ci.IsStatic);
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

    /// <summary>
    /// Scans all assemblies for types.
    /// </summary>
    /// <returns>The <see cref="IEnumerable{T}"/> of types found.</returns>
    public IEnumerable<Type> ScanAssemblies()
    {
        return Assemblies.SelectMany(GetTypes);
    }
}
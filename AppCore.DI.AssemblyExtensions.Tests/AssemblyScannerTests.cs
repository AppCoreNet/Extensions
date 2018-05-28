﻿// Licensed under the MIT License.
// Copyright (c) 2018 the AppCore .NET project.

using System.Reflection;
using FluentAssertions;
using Xunit;

namespace AppCore.DependencyInjection
{
    public class AssemblyScannerTests
    {
        [Fact]
        public void FindsAllImplementationsOfInterface()
        {
            var scanner = new AssemblyScanner(
                typeof(IContract));

            scanner.Filters.Clear();
            scanner.Assemblies.Add(typeof(AssemblyScannerTests).GetTypeInfo().Assembly);

            scanner.ScanAssemblies()
                   .ShouldBeEquivalentTo(
                       new[]
                       {
                           typeof(ContractImpl1),
                           typeof(ContractImpl2)
                       });
        }

        [Fact]
        public void FindsAllImplementationsOfOpenGenericInterface()
        {
            var scanner = new AssemblyScanner(
                typeof(IContract<>));

            scanner.Filters.Clear();
            scanner.Assemblies.Add(typeof(AssemblyScannerTests).GetTypeInfo().Assembly);

            scanner.ScanAssemblies()
                   .ShouldBeEquivalentTo(
                       new[]
                       {
                           typeof(ContractImpl1<>),
                           typeof(ContractImpl2<>),
                           typeof(ContractImpl1String),
                           typeof(ContractImpl2String)
                       });
        }

        [Fact]
        public void FindsAllImplementationsOfClosedGenericInterface()
        {
            var scanner = new AssemblyScanner(
                typeof(IContract<string>));

            scanner.Filters.Clear();
            scanner.Assemblies.Add(typeof(AssemblyScannerTests).GetTypeInfo().Assembly);

            scanner.ScanAssemblies()
                   .ShouldBeEquivalentTo(
                       new[]
                       {
                           typeof(ContractImpl1String),
                           typeof(ContractImpl2String)
                       });
        }

        [Fact]
        public void FindsAllImplementationsOfClass()
        {
            var scanner = new AssemblyScanner(
                typeof(ContractImplBase));

            scanner.Filters.Clear();
            scanner.Assemblies.Add(typeof(AssemblyScannerTests).GetTypeInfo().Assembly);

            scanner.ScanAssemblies()
                   .ShouldBeEquivalentTo(
                       new[]
                       {
                           typeof(ContractImpl1),
                           typeof(ContractImpl2)
                       });
        }

        [Fact]
        public void FindsAllImplementationsOfOpenGenericClass()
        {
            var scanner = new AssemblyScanner(
                typeof(ContractImplBase<>));

            scanner.Filters.Clear();
            scanner.Assemblies.Add(typeof(AssemblyScannerTests).GetTypeInfo().Assembly);

            scanner.ScanAssemblies()
                   .ShouldBeEquivalentTo(
                       new[]
                       {
                           typeof(ContractImpl1<>),
                           typeof(ContractImpl2<>),
                           typeof(ContractImpl1String),
                           typeof(ContractImpl2String)
                       });
        }

        [Fact]
        public void FindsAllImplementationsOfClosedGenericClass()
        {
            var scanner = new AssemblyScanner(
                typeof(ContractImplBaseString));

            scanner.Filters.Clear();
            scanner.Assemblies.Add(typeof(AssemblyScannerTests).GetTypeInfo().Assembly);

            scanner.ScanAssemblies()
                   .ShouldBeEquivalentTo(
                       new[]
                       {
                           typeof(ContractImpl1String),
                           typeof(ContractImpl2String)
                       });
        }
    }
}

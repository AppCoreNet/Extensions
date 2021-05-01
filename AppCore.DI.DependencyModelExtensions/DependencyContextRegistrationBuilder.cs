// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using AppCore.Diagnostics;
using Microsoft.Extensions.DependencyModel;

namespace AppCore.DependencyInjection
{
    /// <summary>
    /// Builds an <see cref="IEnumerable{T}"/> of <see cref="ComponentRegistration"/> by scanning assemblies in a
    /// <see cref="DependencyContext"/>.
    /// </summary>
    public class DependencyContextRegistrationBuilder
    {
        private readonly AssemblyRegistrationBuilder _builder;

        internal DependencyContextRegistrationBuilder(Type contractType)
        {
            _builder = AssemblyRegistrationBuilder.ForContract(contractType);
        }

        public static DependencyContextRegistrationBuilder ForContract(Type contractType)
        {
            return new DependencyContextRegistrationBuilder(contractType);
        }

        public static DependencyContextRegistrationBuilder ForContract<TContract>()
            where TContract : class
        {
            return new DependencyContextRegistrationBuilder(typeof(TContract));
        }

        public DependencyContextRegistrationBuilder From(DependencyContext dependencyContext)
        {
            Ensure.Arg.NotNull(dependencyContext, nameof(dependencyContext));

            _builder.From(
                dependencyContext.GetDefaultAssemblyNames()
                                 .Select(Assembly.Load));

            return this;
        }

        public DependencyContextRegistrationBuilder WithDefaultLifetime(ComponentLifetime lifetime)
        {
            _builder.WithDefaultLifetime(lifetime);
            return this;
        }

        public DependencyContextRegistrationBuilder Filter(Predicate<Type> filter)
        {
            _builder.Filter(filter);
            return this;
        }

        public DependencyContextRegistrationBuilder ClearFilters()
        {
            _builder.ClearFilters();
            return this;
        }

        /// <summary>
        /// Clears the assembly scanner default type filters.
        /// </summary>
        /// <returns>The <see cref="DependencyContextRegistrationBuilder"/>.</returns>
        public DependencyContextRegistrationBuilder ClearDefaultFilters()
        {
            _builder.ClearDefaultFilters();
            return this;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public IEnumerable<ComponentRegistration> BuildRegistrations()
        {
            return _builder.BuildRegistrations();
        }
    }
}
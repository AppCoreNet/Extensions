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
    public class DependencyContextRegistrationBuilder
    {
        private readonly AssemblyRegistrationBuilder _builder = new AssemblyRegistrationBuilder();

        public DependencyContextRegistrationBuilder ForType(Type contractType)
        {
            _builder.ForType(contractType);
            return this;
        }

        public DependencyContextRegistrationBuilder ForType<TContract>()
            where TContract : class
        {
            _builder.ForType<TContract>();
            return this;
        }

        public DependencyContextRegistrationBuilder WithDependencyContext(DependencyContext dependencyContext)
        {
            Ensure.Arg.NotNull(dependencyContext, nameof(dependencyContext));

            _builder.WithAssemblies(
                dependencyContext.GetDefaultAssemblyNames()
                                 .Select(Assembly.Load));

            return this;
        }

        public DependencyContextRegistrationBuilder UseDefaultLifetime(ComponentLifetime lifetime)
        {
            _builder.UseDefaultLifetime(lifetime);
            return this;
        }

        public DependencyContextRegistrationBuilder WithFilter(Predicate<Type> filter)
        {
            _builder.WithFilter(filter);
            return this;
        }

        public DependencyContextRegistrationBuilder ClearFilters()
        {
            _builder.ClearFilters();
            return this;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public IEnumerable<ComponentRegistration> BuildRegistrations()
        {
            return _builder.BuildRegistrations();
        }
    }
}
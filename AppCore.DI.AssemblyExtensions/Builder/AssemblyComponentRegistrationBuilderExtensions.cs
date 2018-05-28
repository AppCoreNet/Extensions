// Licensed under the MIT License.
// Copyright (c) 2018 the AppCore .NET project.

using System;
using AppCore.Diagnostics;

namespace AppCore.DependencyInjection.Builder
{
    public static class AssemblyComponentRegistrationBuilderExtensions
    {
        public static IComponentRegistrationBuilder<AssemblyRegistrationInfo> WithFilter(
            this IComponentRegistrationBuilder<AssemblyRegistrationInfo> builder,
            Predicate<Type> filter)
        {
            Ensure.Arg.NotNull(builder, nameof(builder));
            Ensure.Arg.NotNull(filter, nameof(filter));

            builder.RegistrationInfo.Filters.Add(filter);
            return builder;
        }
    }
}

// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using System.Reflection;
using AppCore.Diagnostics;

namespace AppCore.Hosting.Plugins
{
    internal class AssemblyInfo
    {
        public string Title { get; }

        public string? Version { get; }

        public string? Description { get; }

        public string? Copyright { get; }

        public AssemblyInfo(Assembly assembly)
        {
            Ensure.Arg.NotNull(assembly);

            string? title = assembly
                    ?.GetCustomAttribute<AssemblyTitleAttribute>()
                    ?.Title;

            if (string.IsNullOrEmpty(title))
            {
                title = assembly?.GetName()
                                .Name;
            }

            Title = title!;

            Description = assembly
                          ?.GetCustomAttribute<AssemblyDescriptionAttribute>()
                          ?.Description;

            Version = assembly
                      ?.GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                      ?.InformationalVersion;

            Copyright = assembly
                        ?.GetCustomAttribute<AssemblyCopyrightAttribute>()
                        ?.Copyright;
        }
    }
}
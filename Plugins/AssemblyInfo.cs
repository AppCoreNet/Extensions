// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using System.Reflection;

namespace AppCore.Hosting.Plugins
{
    internal class AssemblyInfo
    {
        public string Title { get; }

        public string Version { get; }

        public string Description { get; }

        public string Copyright { get; }

        public AssemblyInfo(Assembly assembly)
        {
            Title = assembly
                    ?.GetCustomAttribute<AssemblyTitleAttribute>()
                    ?.Title;

            if (string.IsNullOrEmpty(Title))
            {
                Title = assembly.GetName()
                                .Name;
            }

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
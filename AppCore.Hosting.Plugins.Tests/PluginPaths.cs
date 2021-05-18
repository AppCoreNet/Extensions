// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

namespace AppCore.Hosting.Plugins
{
    public static class PluginPaths
    {
#if NETCOREAPP3_1
        private const string FrameworkPath = "netcoreapp3.1";
#endif
#if NET5_0
        private const string FrameworkPath = "net5";
#endif

#if DEBUG
        private const string BuildConfigPath = "Debug";
#else
        private const string BuildConfigPath = "Release";
#endif

        public const string TestPlugin =
            "../../../../AppCore.Hosting.Plugins.TestPlugin/bin/"
            + BuildConfigPath
            + "/"
            + FrameworkPath
            + "/AppCore.Hosting.Plugins.TestPlugin.dll";
    }
}
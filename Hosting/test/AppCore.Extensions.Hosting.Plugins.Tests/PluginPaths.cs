// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

namespace AppCore.Hosting.Plugins
{
    public static class PluginPaths
    {
#if NETCOREAPP3_1
        private const string FrameworkPath = "netcoreapp3.1";
#endif
#if NET6_0
        private const string FrameworkPath = "net6";
#endif

#if DEBUG
        private const string BuildConfigPath = "Debug";
#else
        private const string BuildConfigPath = "Release";
#endif

        public const string TestPlugin =
            "../../../../Plugins.TestPlugin/bin/"
            + BuildConfigPath
            + "/"
            + FrameworkPath
            + "/AppCore.Hosting.Plugins.TestPlugin.dll";

        public const string TestPlugin2 =
            "../../../../Plugins.TestPlugin2/bin/"
            + BuildConfigPath
            + "/"
            + FrameworkPath
            + "/AppCore.Hosting.Plugins.TestPlugin2.dll";
    }
}
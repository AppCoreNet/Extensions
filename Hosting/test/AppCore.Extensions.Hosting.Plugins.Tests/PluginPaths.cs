// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

namespace AppCore.Extensions.Hosting.Plugins
{
    public static class PluginPaths
    {
#if NETCOREAPP3_1
        private const string FrameworkPath = "netcoreapp3.1";
#elif NET6_0
        private const string FrameworkPath = "net6";
#else
    #error Unhandled target framework.
#endif

#if DEBUG
        private const string BuildConfigPath = "Debug";
#else
        private const string BuildConfigPath = "Release";
#endif

        public const string TestPlugin =
            "../../../../AppCore.Extensions.Hosting.Plugins.TestPlugin/bin/"
            + BuildConfigPath
            + "/"
            + FrameworkPath
            + "/AppCore.Extensions.Hosting.Plugins.TestPlugin.dll";

        public const string TestPlugin2 =
            "../../../../AppCore.Extensions.Hosting.Plugins.TestPlugin2/bin/"
            + BuildConfigPath
            + "/"
            + FrameworkPath
            + "/AppCore.Extensions.Hosting.Plugins.TestPlugin2.dll";
    }
}
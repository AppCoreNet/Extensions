// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

namespace AppCoreNet.Extensions.Hosting.Plugins;

public static class PluginPaths
{
#if NET8_0
    private const string FrameworkPath = "net8.0";
#elif NET6_0
    private const string FrameworkPath = "net6.0";
#else
    #error Unhandled target framework.
#endif

#if DEBUG
    private const string BuildConfigPath = "Debug";
#else
    private const string BuildConfigPath = "Release";
#endif

    public const string TestPlugin =
        "../../../../AppCoreNet.Extensions.Hosting.Plugins.TestPlugin/bin/"
        + BuildConfigPath
        + "/"
        + FrameworkPath
        + "/AppCoreNet.Extensions.Hosting.Plugins.TestPlugin.dll";

    public const string TestPlugin2 =
        "../../../../AppCoreNet.Extensions.Hosting.Plugins.TestPlugin2/bin/"
        + BuildConfigPath
        + "/"
        + FrameworkPath
        + "/AppCoreNet.Extensions.Hosting.Plugins.TestPlugin2.dll";
}
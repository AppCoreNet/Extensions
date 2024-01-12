// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AppCoreNet.Diagnostics;
using AppCoreNet.Extensions.DependencyInjection;
using AppCoreNet.Extensions.DependencyInjection.Activator;
using McMaster.NETCore.Plugins;
using Microsoft.Extensions.DependencyInjection;

namespace AppCoreNet.Extensions.Hosting.Plugins;

internal sealed class Plugin : IPlugin, IServiceProviderIsService
{
    private readonly IActivator _activator;
    private readonly PluginOptions _options;

    private static readonly MethodInfo _enumerableOfTypeMethod = typeof(Enumerable).GetMethod(
        nameof(Enumerable.OfType),
        BindingFlags.Public | BindingFlags.Static) !;

    private static readonly ConcurrentDictionary<Type, Func<IEnumerable, IEnumerable>> _enumerableOfTypeMethodCache = new ();

    public PluginLoader Loader { get; }

    public PluginInfo Info
    {
        get
        {
            var assemblyInfo = new AssemblyInfo(Assembly);
            return new PluginInfo(
                assemblyInfo.Title,
                assemblyInfo.Version,
                assemblyInfo.Description,
                assemblyInfo.Copyright);
        }
    }

    public Assembly Assembly { get; }

    public Plugin(PluginLoader loader, IActivator activator, PluginOptions options)
    {
        _activator = activator;
        _options = options;

        Loader = loader;
        Assembly = loader.LoadDefaultAssembly();
    }

    private static IEnumerable CastToServiceEnumerable(IEnumerable list, Type serviceType)
    {
        Func<IEnumerable, IEnumerable> action = _enumerableOfTypeMethodCache.GetOrAdd(
            serviceType,
            t =>
            {
                MethodInfo method = _enumerableOfTypeMethod.MakeGenericMethod(t);
                return (Func<IEnumerable, IEnumerable>)method.CreateDelegate(
                    typeof(Func<IEnumerable, IEnumerable>));
            });

        return action(list);
    }

    public object? GetService(Type serviceType)
    {
        Ensure.Arg.NotNull(serviceType);

        if (serviceType == typeof(IServiceProvider) || serviceType == typeof(IServiceProviderIsService))
            return this;

        bool isEnumerable = false;
        if (serviceType.IsGenericType && serviceType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
        {
            isEnumerable = true;
            serviceType = serviceType.GetGenericArguments()[0];
        }

        var scanner = new AssemblyScanner(serviceType, new[] { Assembly });
        scanner.IncludePrivateTypes = _options.ResolvePrivateTypes;
        scanner.Filters.Clear();

        object? instance = null;
        if (isEnumerable)
        {
            var result = new List<object>();
            foreach (Type type in scanner.ScanAssemblies())
            {
                try
                {
                    result.Add(_activator.CreateInstance(type) !);
                }
                catch (Exception error)
                {
                    Console.Error.WriteLine($"Error activating plugin type '{type.FullName}': {error.Message}");
                }
            }

            instance = CastToServiceEnumerable(result, serviceType);
        }
        else
        {
            foreach (Type type in scanner.ScanAssemblies())
            {
                try
                {
                    instance = _activator.CreateInstance(type);
                    break;
                }
                catch (Exception error)
                {
                    Console.Error.WriteLine($"Error activating plugin type '{type.FullName}': {error.Message}");
                }
            }
        }

        return instance;
    }

    public bool IsService(Type serviceType)
    {
        if (serviceType == typeof(IServiceProvider) || serviceType == typeof(IServiceProviderIsService))
            return true;

        if (serviceType.IsGenericType && serviceType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
            return true;

        var scanner = new AssemblyScanner(serviceType, new[] { Assembly });
        scanner.IncludePrivateTypes = _options.ResolvePrivateTypes;
        scanner.Filters.Clear();

        return scanner.ScanAssemblies().Any();
    }

    public Assembly LoadAssembly(string fileName)
    {
        return Loader.LoadAssembly(fileName);
    }

    public IDisposable EnterContextualReflection()
    {
        return Loader.EnterContextualReflection();
    }
}
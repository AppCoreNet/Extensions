using System.Collections.Generic;

namespace AppCore.Extensions.Hosting.Plugins;

/// <summary>
/// Represents a collection of plugin services.
/// </summary>
/// <typeparam name="T">The type of the service.</typeparam>
public interface IPluginServiceCollection<out T> : IEnumerable<IPluginService<T>>
{
}
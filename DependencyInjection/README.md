AppCore .NET add-ons for Microsoft.Extensions.DependencyInjection
--------------------

This project contains add-ons for the `Microsoft.Extensions.DependencyInjection` library. It adds support for registering services with
the `IServiceCollection` by reflection.

## Packages

| Package                                                            | Description                                                                           |
|--------------------------------------------------------------------|---------------------------------------------------------------------------------------|
| `AppCore.Extensions.DependencyInjection.Abstractions`              | Provides the abstractions for registering services via reflection.                    |
| `AppCore.Extensions.DependencyInjection.AssemblyExtensions`        | Adds support for registering services by scanning assemblies.                         |
| `AppCore.Extensions.DependencyInjection.DependencyModelExtensions` | Adds support for registering services by scanning assemblies of a `DependencyContext` |

## Usage

The library provides several extensions methods for the `IServiceCollection` interface:

### AddFrom

This method is analog to `IServiceCollection.Add()` but instead of specifying the implementation type explicitly
it will resolve them using reflection.

Example:

```csharp
services.AddFrom<IMyService>(r => r.Assembly());
```

(NOTE: the Assembly() method will be available after adding the AssemblyExtensions package.)

This will scan the current assembly for all implementations of the `IMyService` type and registers them with
the `IServiceCollection`.

By default the services are registered with the `ServiceLifetime.Transient` lifetime, to change the default
you can call `WithDefaultLifetime()`. Alternatively the service implementation can be decorated with the
`LifetimeAttribute`.

### TryAddFrom

This method provides the same features than `AddFrom()` but will skip services which have already been registered
with the same service type.

### TryAddEnumerableFrom

This method provides the same features than `AddFrom()` but will skip services which have already been registered
with the same service and implementation type.


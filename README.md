AppCore .NET add-ons for Microsoft.Extensions
--------------------

[![Build Status](https://dev.azure.com/AppCoreNet/Extensions/_apis/build/status/AppCoreNet.Extensions%20CI?branchName=dev)](https://dev.azure.com/AppCoreNet/Extensions/_build/latest?definitionId=7&branchName=dev)
![Azure DevOps tests (compact)](https://img.shields.io/azure-devops/tests/AppCoreNet/Extensions/7?compact_message)
![Azure DevOps coverage (branch)](https://img.shields.io/azure-devops/coverage/AppCoreNet/Extensions/7/dev)
![Nuget](https://img.shields.io/nuget/v/AppCore.Extensions.DependencyInjection.Abstractions)

This repository contains add-ons for the various `Microsoft.Extensions` libraries. It targets the .NET Framework and .NET Core.

All artifacts are licensed under the [MIT license](LICENSE). You are free to use them in open-source or commercial projects as long as you keep the copyright notice intact when redistributing or otherwise reusing our artifacts.

## Packages

Latest development packages can be found on [MyGet](https://www.myget.org/gallery/appcorenet).

| Package                                                 | Description                                                                  |
|---------------------------------------------------------|------------------------------------------------------------------------------|
| `AppCore.DependencyInjection.Abstractions`              | Provides add-ons for `Microsoft.DependencyInjection.Abstractions`            |
| `AppCore.DependencyInjection.AssemblyExtensions`        | Provides extensions to register services via assembly reflection.            |
| `AppCore.DependencyInjection.DependencyModelExtensions` | Provides extensions to register services via `DependencyContext` reflection. |
| `AppCore.Hosting.Abstractions`                          | Provides add-ons for `Microsoft.Extensions.Hosting`                          |
| `AppCore.Hosting`                                       |                                                                              |
| `AppCore.Hosting.Plugins.Abstractions`                  | Provides the public API for hosting plugins.                                 |
| `AppCore.Hosting.Plugins`                               | Default implementation of the plugins hosting API.                           |


## Contributing

Contributions, whether you file an issue, fix some bug or implement a new feature, are highly appreciated. The whole user community
will benefit from them.

Please refer to the [Contribution guide](CONTRIBUTING.md).

AppCore .NET add-ons for Microsoft.Extensions
--------------------

[![Build Status](https://dev.azure.com/AppCoreNet/Extensions/_apis/build/status/AppCore.Extensions%20CI?branchName=dev)](https://dev.azure.com/AppCoreNet/Extensions/_build/latest?definitionId=19&branchName=dev)
![Azure DevOps tests (compact)](https://img.shields.io/azure-devops/tests/AppCoreNet/Extensions/19?compact_message)
![Azure DevOps coverage (branch)](https://img.shields.io/azure-devops/coverage/AppCoreNet/Extensions/19/dev)
![Nuget](https://img.shields.io/nuget/v/AppCore.Extensions.DependencyInjection.Abstractions)
![MyGet](https://img.shields.io/myget/appcorenet/vpre/AppCore.Extensions.DependencyInjection.Abstractions?label=myget)

This repository contains add-ons for the various `Microsoft.Extensions` libraries. It targets the .NET Framework and .NET Core.

All artifacts are licensed under the [MIT license](LICENSE). You are free to use them in open-source or commercial projects as long as you keep the copyright notice intact when redistributing or otherwise reusing our artifacts.

Latest development packages can be found on [MyGet](https://www.myget.org/gallery/appcorenet).

## Projects

| Project                                                                 | Description                                                                                                                         |
|-------------------------------------------------------------------------|-------------------------------------------------------------------------------------------------------------------------------------|
| [AppCore.Extensions.DependencyInjection](DependencyInjection/README.md) | Provides add-ons for `Microsoft.Extensions.DependencyInjection`. It contains extension methods to register services via reflection. |
| [AppCore.Extensions.Hosting](Hosting/README.md)                         | Add-ons for `Microsoft.Extensions.Hosting`. Provides support for dynamically loading plugins and registering services.              |
| [AppCore.Extensions.Http](Http/README.md)                               | Extends `Microsoft.Extensions.Http` adding support for client authentication.                                                       |

## Contributing

Contributions, whether you file an issue, fix some bug or implement a new feature, are highly appreciated. The whole user community
will benefit from them.

Please refer to the [Contribution guide](CONTRIBUTING.md).

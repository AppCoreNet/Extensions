AppCore .NET add-ons for Microsoft.Extensions.Hosting
--------------------

This project contains add-ons for the `Microsoft.Extensions.Hosting` library. It adds support for running tasks on application startup
and dynamically loading plugins into the hosting application.

## Packages

| Package                                             | Description                                                   |
|-----------------------------------------------------|---------------------------------------------------------------|
| `AppCore.Extensions.Hosting.Abstractions`           | Provides the public API for the hosting extensions.           |
| `AppCore.Extensions.Hosting`                        | The default implementations, used by the hosting application. |
| `AppCore.Extensions.Hosting.Plugins.Abstractions`   | Provides the public API for dynamically loading plugins.      |
| `AppCore.Extensions.Hosting.Plugins`                | Default implementation of the plugin APIs.                    |
| `AppCore.Extensions.Hosting.Plugins.AspNetCore.Mvc` | Adds support for registering plugins with MVC.                |

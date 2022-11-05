AppCore .NET add-ons for Microsoft.Extensions.Http
--------------------

This project contains add-ons for the `Microsoft.Extensions.Http` library. It adds authentication support to the `IHttpCLientFactory`.

## Features


## Packages

Latest development packages can be found on [MyGet](https://www.myget.org/gallery/appcorenet).

| Package                                                                 | Description                                                                                                       |
|-------------------------------------------------------------------------|-------------------------------------------------------------------------------------------------------------------|
| `AppCore.Extensions.Http.Authentication`                                | Provides extensions which adds support for authenticating a HttpClient using different authentication standards.  |
| `AppCore.Extensions.Http.Authentication.OAuth`                          | Adds support for authenticating a HttpClient using OAuth2 bearer tokens.                                          |
| `AppCore.Extensions.Http.Authentication.OAuth.AspNetCore.OpenIdConnect` | Adds support for deriving token client configuration from ASP.NET Core OpenID connect authentication schemes.     |

## Usage

To add transparent authentication to a `HttpClient` which is built using the `IHttpClientFactory` you first have to register a client authentication scheme.
Currently OAuth2 client credentials and password flow is supported out-of-the-box. Custom schemes can be easily added.

After registering the scheme you have to configure the `IHttpClientFactory` to use the authentication handler. That's all.

Add the package to your project:

```shell
> dotnet add package AppCore.Extensions.Http.Authentication.OAuth
```

Register a client authentication scheme, the following sample uses client credential authentication.

```csharp
services.AddHttpClientAuthentication()
        .AddOAuthClient("catalog.client",
                        o => {
                          o.TokenEndpoint = "https://demo.duendesoftware.com/connect/token";
                          o.ClientId = "6f59b670-990f-4ef7-856f-0dd584ed1fac";
                          o.ClientSecret = "d0c17c6a-ba47-4654-a874-f6d576cdf799";
                          o.Scope = "catalog inventory";
                        });
```

Configure your HTTP client to use the authentication scheme:

```csharp
services.AddHttpClient("CatalogClient")
        .AddOAuthClientAuthentication("catalog.client");
```

For frontend apps it is also possible to infer the OAuth client configuration from the ASP.Net Core
OIDC authentication scheme.

```csharp
services.AddHttpClientAuthentication()
        .AddOAuthClientForScheme(o => { o.OpenIdConnect() });
```
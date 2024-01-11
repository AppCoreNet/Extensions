using System;
using AppCore.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WorkerService;

IHost host =
    Host.CreateDefaultBuilder(args)
        .ConfigureServices(
            services =>
            {
                services.AddHttpClientAuthentication()
                        .AddOAuthClient(
                            "client",
                            o =>
                            {
                                o.TokenEndpoint = new Uri("https://demo.duendesoftware.com/connect/token");
                                o.ClientId = "m2m.short";
                                o.ClientSecret = "secret";
                                o.Scope = "api";
                            });

                services.AddHttpClient(
                            "api-client",
                            client =>
                            {
                                client.BaseAddress = new Uri("https://demo.duendesoftware.com/api/");
                            })
                        .AddOAuthClientAuthentication("client");

                services.AddHostedService<Worker>();
            })
        .Build();

await host.RunAsync();
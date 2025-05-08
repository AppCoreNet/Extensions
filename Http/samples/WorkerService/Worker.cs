using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace WorkerService;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IHttpClientFactory _factory;

    public Worker(ILogger<Worker> logger, IHttpClientFactory factory)
    {
        _logger = logger;
        _factory = factory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

            using HttpClient client = _factory.CreateClient("api-client");
            using HttpResponseMessage response = await client.GetAsync("test", stoppingToken);

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync(stoppingToken);
                _logger.LogInformation("API response: {response}", content);
            }
            else
            {
                _logger.LogError("API returned: {statusCode}", response.StatusCode);
            }

            await Task.Delay(5000, stoppingToken);
        }
    }
}
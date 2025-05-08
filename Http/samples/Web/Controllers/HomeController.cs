using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Web.Models;

namespace Web.Controllers;

[SuppressMessage(
    "IDisposableAnalyzers.Correctness",
    "IDISP001:Dispose created",
    Justification = "HttpClient instances are managed by IHttpClientFactory")]
public class HomeController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<HomeController> _logger;

    public HomeController(IHttpClientFactory httpClientFactory, ILogger<HomeController> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [AllowAnonymous]
    public async Task<IActionResult> CallApiAsClientAsync()
    {
        HttpClient client = _httpClientFactory.CreateClient("api-client");

        string response = await client.GetStringAsync("test");
        ViewBag.Json = JsonNode.Parse(response)!.ToString();

        return View("CallApi");
    }

    [Authorize]
    public async Task<IActionResult> CallApiAsUserAsync()
    {
        HttpClient client = _httpClientFactory.CreateClient("api-user-client");

        string response = await client.GetStringAsync("test");
        ViewBag.Json = JsonNode.Parse(response)!.ToString();

        return View("CallApi");
    }
}
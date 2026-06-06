using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using marsian_library.Models;
using marsian_library.Services;

namespace marsian_library.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly WeatherRaportService _weatherService;

    public HomeController(ILogger<HomeController> logger, WeatherRaportService weatherService)
    {
        _logger = logger;
        _weatherService = weatherService;
    }

    public async Task<IActionResult> Index()
    {
        var weatherData = await _weatherService.GetWeatherAsync();
        return View(weatherData);
    }
    public IActionResult BookCrud()
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
}

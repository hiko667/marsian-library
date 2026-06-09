using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using marsian_library.Models;
using Microsoft.AspNetCore.Authorization;
using marsian_library.Services;
using Microsoft.AspNetCore.Authorization;

namespace marsian_library.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly WeatherRaportService _weatherService;
    private readonly IWebHostEnvironment _env;

    public HomeController(ILogger<HomeController> logger, WeatherRaportService weatherService, IWebHostEnvironment env)
    {
        _logger = logger;
        _weatherService = weatherService;
        _env = env;
    }

    public async Task<IActionResult> Index()
    {
        var weatherData = await _weatherService.GetWeatherAsync();
        return View(weatherData);
    }
    
    [Authorize(Roles = "Employee, Admin")]
    public IActionResult Operations()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }
    public IActionResult About()
    {
        return View();
    }
    public IActionResult Informations()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

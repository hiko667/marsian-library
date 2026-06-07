using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using marsian_library.Models;
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

    [HttpPost]
    [Authorize(Roles = "Employee,Admin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UploadFile(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            TempData["ErrorMessage"] = "Nie wybrano pliku lub plik jest pusty.";
            return RedirectToAction("BookCrud");
        }

        try
        {
            string uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            string uniqueFileName = file.FileName;
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            TempData["SuccessMessage"] = $"Plik '{uniqueFileName}' został pomyślnie przesłany na serwer!";
        }
        catch (System.Exception ex)
        {
            TempData["ErrorMessage"] = $"Błąd podczas zapisu pliku: {ex.Message}";
        }

        return RedirectToAction("BookCrud");
    }
}

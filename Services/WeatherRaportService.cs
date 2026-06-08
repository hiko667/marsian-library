using System.Text.Json;
using marsian_library.Models;

namespace marsian_library.Services;
public class WeatherRaportService
{
    private readonly HttpClient _httpClient;
    //link z kluczem API uzyskanym ze strony NASA
    private const string ApiUrl = "https://api.nasa.gov/insight_weather/?api_key=9DXKhQQ4I9eXgLuEFl5JwfVdPoQGf9MhxFIApSb8&feedtype=json&ver=1.0";
    public WeatherRaportService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    public async Task<WeatherRaport> GetWeatherAsync()
    {
        var response = await _httpClient.GetAsync(ApiUrl);
        response.EnsureSuccessStatusCode();

        var jsonString = await response.Content.ReadAsStringAsync();
        
        // Deserializacja podstawowa
        var data = JsonSerializer.Deserialize<WeatherRaport>(jsonString);

        if (data != null && data.SolKeys != null)
        {
            foreach (var key in data.SolKeys)
            {
                if (data.RawSols.TryGetValue(key, out var solElement))
                {
                    var solJson = solElement.ToString();
                    if (solJson != null)
                    {
                        var solData = JsonSerializer.Deserialize<SolData>(solJson);
                        if (solData != null)
                        {
                            data.Sols.Add(key, solData);
                        }
                    }
                }
            }
        }

        return data ?? new WeatherRaport();
    }
}
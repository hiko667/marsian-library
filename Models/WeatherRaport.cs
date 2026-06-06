using System.Text.Json.Serialization;

namespace marsian_library.Models;

public class WeatherRaport
{
    [JsonPropertyName("sol_keys")]
    public List<string> SolKeys { get; set; } = new();

    [JsonExtensionData]
    public Dictionary<string, object> RawSols { get; set; } = new();

    public Dictionary<string, SolData> Sols { get; set; } = new();
}

public class SolData
{
    [JsonPropertyName("AT")]
    public MetricData? AirTemperature { get; set; }

    [JsonPropertyName("HWS")]
    public MetricData? HorizontalWindSpeed { get; set; }

    [JsonPropertyName("PRE")]
    public MetricData? AtmosphericPressure { get; set; }

    [JsonPropertyName("Season")]
    public string? Season { get; set; }
}

public class MetricData
{
    [JsonPropertyName("av")]
    public double Average { get; set; }

    [JsonPropertyName("mn")]
    public double Minimum { get; set; }

    [JsonPropertyName("mx")]
    public double Maximum { get; set; }
}
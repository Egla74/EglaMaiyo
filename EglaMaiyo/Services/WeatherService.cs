using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using EglaMaiyo.Models;

namespace EglaMaiyo.Services
{
    public class WeatherService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<WeatherService> _logger;
        private readonly LogService _logService;

        private const string ApiKey = "2d57f14922ea34864bd04aa88ebb1caa"; 

        public WeatherService(HttpClient httpClient, ILogger<WeatherService> logger, LogService logService)
        {
            _httpClient = httpClient;
            _logger = logger;
            _logService = logService;
        }

        public async Task<WeatherInfo?> GetWeatherAsync(string city)
        {
            try
            {
                var url = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={ApiKey}&units=metric";
                System.Console.WriteLine($"[DEBUG] Fetching weather for: {city}");
                System.Console.WriteLine($"[DEBUG] URL: {url}");

                var result = await _httpClient.GetFromJsonAsync<ApiWeatherResponse>(url);

                if (result == null || result.main == null || result.weather == null || result.weather.Count == 0)
                {
                    System.Console.WriteLine("[DEBUG] Weather API returned no data.");
                    _logService.AddLog("Weather API", "Fail", $"No data returned for city: {city}");
                    return null;
                }

                var weatherInfo = new WeatherInfo
                {
                    City = city,
                    TemperatureCelsius = result.main.temp,
                    Description = result.weather[0].description
                };

                _logService.AddLog("Weather API", "Success", $"Weather fetched for {city}");
                return weatherInfo;
            }
            catch (HttpRequestException httpEx)
            {
                System.Console.WriteLine($"[ERROR] HTTP Exception: {httpEx.Message}");
                _logger.LogError(httpEx, "HTTP error fetching weather for {City}", city);
                _logService.AddLog("Weather API", "Error", $"HTTP exception for {city}: {httpEx.Message}");
                return null;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"[ERROR] General Exception: {ex.Message}");
                _logger.LogError(ex, "Error fetching weather for {City}", city);
                _logService.AddLog("Weather API", "Error", $"Exception for {city}: {ex.Message}");
                return null;
            }
        }

        // JSON mapping based on OpenWeatherMap API
        private class ApiWeatherResponse
        {
            public WeatherMain main { get; set; }
            public List<WeatherDescription> weather { get; set; }
        }

        private class WeatherMain
        {
            public float temp { get; set; }
        }

        private class WeatherDescription
        {
            public string description { get; set; }
        }
    }
}

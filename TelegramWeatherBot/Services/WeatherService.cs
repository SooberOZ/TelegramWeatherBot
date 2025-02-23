using Newtonsoft.Json;
using TelegramWeatherBot.Model;

namespace TelegramWeatherBot.Services
{
    public class WeatherService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _baseUrl;

        public WeatherService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _apiKey = config["WeatherApi:ApiKey"];
            _baseUrl = config["WeatherApi:BaseUrl"];
        }

        public async Task<WeatherResponse?> GetWeather(string city)
        {
            var url = $"{_baseUrl}?q={city}&appid={_apiKey}&units=metric";
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<WeatherResponse>(json);
        }
    }
}
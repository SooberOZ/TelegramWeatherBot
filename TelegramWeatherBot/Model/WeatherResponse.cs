using Newtonsoft.Json;

namespace TelegramWeatherBot.Model
{
    public class WeatherResponse
    {
        [JsonProperty("main")]
        public WeatherTemp Main { get; set; }

        [JsonProperty("weather")]
        public List<WeatherDescription> Weather { get; set; }
    }
}
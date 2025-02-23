using System.Globalization;
using TelegramWeatherBot.Repositories;

namespace TelegramWeatherBot.Services
{
    public class BotCommandHandler
    {
        private readonly IUserRepository _userRepo;
        private readonly IWeatherHistoryRepository _weatherHistoryRepo;
        private readonly WeatherService _weatherService;

        public BotCommandHandler(
            IUserRepository userRepo,
            IWeatherHistoryRepository weatherHistoryRepo,
            WeatherService weatherService)
        {
            _userRepo = userRepo;
            _weatherHistoryRepo = weatherHistoryRepo;
            _weatherService = weatherService;
        }

        public async Task<string> HandleWeatherCommandAsync(long chatId, string city)
        {
            var user = await _userRepo.GetUserByTelegramIdAsync(chatId);
            if (user == null)
            {
                return "Please write /start, for regestration.";
            }

            var weatherData = await _weatherService.GetWeather(city);
            if (weatherData == null)
            {
                return "No, please, write your city (seems, I can't find that city).";
            }

            await _weatherHistoryRepo.SaveWeatherAsync(
                user.Id, city, weatherData.Main.Temp, weatherData.Weather[0].Description);

            return $"Weather in {city}: {weatherData.Main.Temp.ToString(CultureInfo.InvariantCulture)}°C, {weatherData.Weather[0].Description}.";
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using Telegram.Bot;
using TelegramWeatherBot.Repositories;
using TelegramWeatherBot.Services;

namespace TelegramWeatherBot.Controllers
{
    public class WeatherController : ControllerBase
    {
        private readonly IUserRepository _userRepo;
        private readonly IWeatherHistoryRepository _weatherHistoryRepo;
        private readonly WeatherService _weatherService;
        private readonly ITelegramBotClient _botClient;

        public WeatherController(IUserRepository userRepo, IWeatherHistoryRepository weatherHistoryRepo, WeatherService weatherService, ITelegramBotClient botClient)
        {
            _userRepo = userRepo;
            _weatherHistoryRepo = weatherHistoryRepo;
            _weatherService = weatherService;
            _botClient = botClient;
        }

        [HttpPost("sendWeatherToAll")]
        public async Task<IActionResult> SendWeatherToAll([FromBody] string city)
        {
            var users = await _userRepo.GetAllUsersAsync();
            foreach (var user in users)
            {
                var weatherData = await _weatherService.GetWeather(city);
                if (weatherData != null)
                {
                    await _weatherHistoryRepo.SaveWeatherAsync(
                        user.Id, city, weatherData.Main.Temp, weatherData.Weather[0].Description);

                    var message = $"Weather in {city}: {weatherData.Main.Temp.ToString(CultureInfo.InvariantCulture)}°C, {weatherData.Weather[0].Description}.";

                    try
                    {
                        await _botClient.SendMessage(user.TelegramId, message);
                    }
                    catch (Telegram.Bot.Exceptions.ApiRequestException ex) when (ex.ErrorCode == 400 || ex.ErrorCode == 403)
                    {
                        Console.WriteLine($"Can't send message to this user {user.TelegramId}: {ex.Message}");
                    }
                }
            }

            return Ok("Message sending completed successfully.");
        }
    }
}
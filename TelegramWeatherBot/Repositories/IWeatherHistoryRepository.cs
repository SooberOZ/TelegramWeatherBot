using TelegramWeatherBot.Model;

namespace TelegramWeatherBot.Repositories
{
    public interface IWeatherHistoryRepository
    {
        Task SaveWeatherAsync(int userId, string city, float temperature, string description);
        Task<List<WeatherHistory>> GetHistoryByUserIdAsync(int userId);
    }
}
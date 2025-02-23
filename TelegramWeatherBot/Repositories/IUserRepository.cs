using TelegramWeatherBot.Model;

namespace TelegramWeatherBot.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetUserByTelegramIdAsync(long telegramId);
        Task<User?> GetUserByIdAsync(int userId);
        Task<int> AddUserAsync(User user);
        Task<List<User>> GetAllUsersAsync();
    }
}
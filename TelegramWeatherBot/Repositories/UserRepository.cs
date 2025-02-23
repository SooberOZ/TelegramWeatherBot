using Dapper;
using System.Data;
using TelegramWeatherBot.Model;

namespace TelegramWeatherBot.Repositories
{
    public class UserRepository: IUserRepository
    {
        private readonly IDbConnection _db;

        public UserRepository(IDbConnection db) => _db = db;

        public async Task<User?> GetUserByTelegramIdAsync(long telegramId)
        {
            var sql = "SELECT * FROM Users WHERE TelegramId = @tgId";
            return await _db.QueryFirstOrDefaultAsync<User>(sql, new { tgId = telegramId });
        }

        public async Task<User?> GetUserByIdAsync(int userId)
        {
            var sql = "SELECT * FROM Users WHERE Id = @id";
            return await _db.QueryFirstOrDefaultAsync<User>(sql, new { id = userId });
        }

        public async Task<int> AddUserAsync(User user)
        {
            var sql = @"INSERT INTO Users (TelegramId, Name) 
                    VALUES (@TelegramId, @Name);
                    SELECT CAST(SCOPE_IDENTITY() as int);";
            return await _db.ExecuteScalarAsync<int>(sql, user);
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            var sql = "SELECT * FROM Users";
            var result = await _db.QueryAsync<User>(sql);
            return result.ToList();
        }
    }
}
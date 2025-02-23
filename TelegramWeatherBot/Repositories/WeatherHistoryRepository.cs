using Dapper;
using System.Data;
using TelegramWeatherBot.Model;

namespace TelegramWeatherBot.Repositories
{
    public class WeatherHistoryRepository: IWeatherHistoryRepository
    {
        private readonly IDbConnection _db;

        public WeatherHistoryRepository(IDbConnection db) => _db = db;

        public async Task SaveWeatherAsync(int userId, string city, float temperature, string description)
        {
            var sql = @"INSERT INTO WeatherHistory (UserId, City, Temperature, Description, Date) 
                    VALUES (@UserId, @City, @Temp, @Desc, GETDATE());";
            await _db.ExecuteAsync(sql, new { UserId = userId, City = city, Temp = temperature, Desc = description });
        }

        public async Task<List<WeatherHistory>> GetHistoryByUserIdAsync(int userId)
        {
            var sql = "SELECT * FROM WeatherHistory WHERE UserId = @uid ORDER BY Date DESC";
            var result = await _db.QueryAsync<WeatherHistory>(sql, new { uid = userId });
            return result.ToList();
        }
    }
}
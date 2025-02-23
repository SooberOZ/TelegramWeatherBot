namespace TelegramWeatherBot.Model
{
    public class WeatherHistory
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string City { get; set; }
        public float Temperature { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
    }
}
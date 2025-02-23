using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramWeatherBot.Repositories;

namespace TelegramWeatherBot.Services
{
    public class TelegramBotService
    {
        private readonly ITelegramBotClient _botClient;
        private readonly IServiceScopeFactory _scopeFactory;

        public TelegramBotService(IConfiguration config, IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
            var token = config["TelegramBot:Token"];
            _botClient = new TelegramBotClient(token);
        }

        public async Task HandleUpdateAsync(Update update)
        {
            if (update?.Message?.Text is null)
            {
                return;
            }

            var chatId = update.Message.Chat.Id;
            var text = update.Message.Text.Trim();

            if (text.Equals("/start", StringComparison.OrdinalIgnoreCase))
            {
                await HandleStartAsync(chatId, update.Message.From);
                return;
            }

            if (text.StartsWith("/weather", StringComparison.OrdinalIgnoreCase))
            {
                var parts = text.Split(' ', 2);
                if (parts.Length < 2)
                {
                    await _botClient.SendMessage(chatId, "Write /weather [city]");
                    return;
                }
                var city = parts[1];

                using var scope = _scopeFactory.CreateScope();
                var commandHandler = scope.ServiceProvider.GetRequiredService<BotCommandHandler>();

                var result = await commandHandler.HandleWeatherCommandAsync(chatId, city);
                await _botClient.SendMessage(chatId, result);
                return;
            }

            using (var scope = _scopeFactory.CreateScope())
            {
                var commandHandler = scope.ServiceProvider.GetRequiredService<BotCommandHandler>();
                var result = await commandHandler.HandleWeatherCommandAsync(chatId, text);
                await _botClient.SendMessage(chatId, result);
            }
        }

        private async Task HandleStartAsync(long chatId, User from)
        {
            using var scope = _scopeFactory.CreateScope();
            var userRepo = scope.ServiceProvider.GetRequiredService<IUserRepository>();

            var user = await userRepo.GetUserByTelegramIdAsync(chatId);
            if (user == null)
            {
                var userName = !string.IsNullOrEmpty(from.Username)
                    ? from.Username
                    : (from.FirstName + " " + from.LastName).Trim();

                var newUser = new Model.User
                {
                    TelegramId = chatId,
                    Name = string.IsNullOrEmpty(userName) ? $"User_{chatId}" : userName
                };
                await userRepo.AddUserAsync(newUser);
            }

            await _botClient.SendMessage(chatId, "Welcome! Please write your city on English , to know your weather.");
        }
    }
}
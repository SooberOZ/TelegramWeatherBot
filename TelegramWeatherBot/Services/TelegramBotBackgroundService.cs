using Telegram.Bot;
using Telegram.Bot.Polling;

namespace TelegramWeatherBot.Services
{
    public class TelegramBotBackgroundService: BackgroundService
    {
        private readonly TelegramBotService _botService;
        private readonly ITelegramBotClient _botClient;

        public TelegramBotBackgroundService(TelegramBotService botService, IConfiguration config)
        {
            _botService = botService;
            var token = config["TelegramBot:Token"];
            _botClient = new TelegramBotClient(token);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var receiverOptions = new ReceiverOptions { AllowedUpdates = { } };

            _botClient.StartReceiving(
                async (ctx, update, ct) => await _botService.HandleUpdateAsync(update),
                async (ctx, ex, ct) => Console.WriteLine($"Error: {ex.Message}"),
                receiverOptions,
                stoppingToken
            );

            await Task.Delay(-1, stoppingToken);
        }
    }
}
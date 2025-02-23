using Microsoft.Data.SqlClient;
using Microsoft.OpenApi.Models;
using System.Data;
using Telegram.Bot;
using TelegramWeatherBot.Repositories;
using TelegramWeatherBot.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IDbConnection>(sp =>
new SqlConnection(builder.Configuration.GetConnectionString("SqlServer")));

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IWeatherHistoryRepository, WeatherHistoryRepository>();
builder.Services.AddScoped<BotCommandHandler>();

builder.Services.AddHttpClient<WeatherService>();
builder.Services.AddSingleton<ITelegramBotClient>(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    var token = config["TelegramBot:Token"];
    return new TelegramBotClient(token);
});

builder.Services.AddSingleton<TelegramBotService>();
builder.Services.AddHostedService<TelegramBotBackgroundService>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "WeatherBot API", Version = "v1" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WeatherBot API v1"));
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

using Microsoft.AspNetCore.Mvc;
using TelegramWeatherBot.Repositories;

namespace TelegramWeatherBot.Controllers;

[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    private readonly IUserRepository _userRepo;
    private readonly IWeatherHistoryRepository _weatherHistoryRepo;

    public UserController(IUserRepository userRepo, IWeatherHistoryRepository weatherHistoryRepo)
    {
        _userRepo = userRepo;
        _weatherHistoryRepo = weatherHistoryRepo;
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetUser(int userId)
    {
        var user = await _userRepo.GetUserByIdAsync(userId);
        if (user == null)
            return NotFound("User not found.");

        var history = await _weatherHistoryRepo.GetHistoryByUserIdAsync(user.Id);

        return Ok(new
        {
            user.Id,
            user.Name,
            WeatherHistory = history

        });
    }
}
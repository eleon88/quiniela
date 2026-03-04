using Microsoft.AspNetCore.Mvc;
using Quiniela.Api.Managers.Interfaces;

namespace Quiniela.Api.Controllers;

[ApiController]
public class LeaderboardController : ControllerBase
{
    private readonly ILeaderboardManager _leaderboardManager;

    public LeaderboardController(ILeaderboardManager leaderboardManager) => _leaderboardManager = leaderboardManager;

    [HttpGet("api/round/{roundId:guid}/leaderboard")]
    public async Task<IActionResult> GetLeaderboard(Guid roundId)
    {
        var leaderboard = await _leaderboardManager.GetLeaderboardAsync(roundId);
        return Ok(leaderboard);
    }
}

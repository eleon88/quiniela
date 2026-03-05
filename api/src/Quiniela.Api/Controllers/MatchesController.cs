using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quiniela.Api.DTOs.Matches;
using Quiniela.Api.Infrastructure;
using Quiniela.Api.Managers.Interfaces;

namespace Quiniela.Api.Controllers;

[ApiController]
public class MatchesController : ControllerBase
{
    private readonly IMatchManager _matchManager;

    public MatchesController(IMatchManager matchManager) => _matchManager = matchManager;

    [HttpGet("api/round/{roundId:guid}/matches")]
    public async Task<IActionResult> GetByRoundId(Guid roundId)
    {
        var matches = await _matchManager.GetByRoundIdAsync(roundId);
        return Ok(matches);
    }

    [HttpPost("api/round/{id:guid}/matches")]
    [Authorize]
    [BoardAdminAuthorize(BoardResourceType.Round)]
    public async Task<IActionResult> CreateBatch(Guid id, [FromBody] CreateMatchesRequest request)
    {
        var matches = await _matchManager.CreateBatchAsync(id, request);
        return Created($"/api/round/{id}/matches", matches);
    }

    [HttpPut("api/match/{id:guid}/result")]
    [Authorize]
    [BoardAdminAuthorize(BoardResourceType.Match)]
    public async Task<IActionResult> UpdateResult(Guid id, [FromBody] UpdateMatchResultRequest request)
    {
        await _matchManager.UpdateResultAsync(id, request.Result);
        return NoContent();
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quiniela.Api.DTOs.Rounds;
using Quiniela.Api.Infrastructure;
using Quiniela.Api.Managers.Interfaces;

namespace Quiniela.Api.Controllers;

[ApiController]
public class RoundsController : ControllerBase
{
    private readonly IRoundManager _roundManager;

    public RoundsController(IRoundManager roundManager) => _roundManager = roundManager;

    [HttpGet("api/rounds/{boardId:guid}")]
    public async Task<IActionResult> GetByBoardId(Guid boardId)
    {
        var rounds = await _roundManager.GetByBoardIdAsync(boardId);
        return Ok(rounds);
    }

    [HttpGet("api/rounds/{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var round = await _roundManager.GetByIdAsync(id);
        return round is null ? NotFound() : Ok(round);
    }

    [HttpPut("api/rounds/{id:guid}/status")]
    [Authorize]
    [BoardAdminAuthorize(BoardResourceType.Round)]
    public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateRoundStatusRequest request)
    {
        await _roundManager.UpdateStatusAsync(id, request.Status);
        return NoContent();
    }
}

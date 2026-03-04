using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quiniela.Api.DTOs.Rounds;
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

    [HttpPut("api/round/{id:guid}/status")]
    [Authorize]
    public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateRoundStatusRequest request)
    {
        await _roundManager.UpdateStatusAsync(id, request.Status);
        return NoContent();
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quiniela.Api.DTOs.Boards;
using Quiniela.Api.DTOs.Rounds;
using Quiniela.Api.Infrastructure;
using Quiniela.Api.Managers.Interfaces;

namespace Quiniela.Api.Controllers;

[ApiController]
[Route("api/boards")]
public class BoardsController : ControllerBase
{
    private readonly IBoardManager _boardManager;
    private readonly IRoundManager _roundManager;
    private readonly IUserManager _userManager;

    public BoardsController(IBoardManager boardManager, IRoundManager roundManager, IUserManager userManager)
    {
        _boardManager = boardManager;
        _roundManager = roundManager;
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var boards = await _boardManager.GetPublicBoardsAsync();
        return Ok(boards);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var board = await _boardManager.GetByIdAsync(id);
        return Ok(board);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] CreateBoardRequest request)
    {
        var auth0Id = Auth0ClaimsHelper.GetAuth0Id(User);
        var user = await _userManager.GetOrCreateByAuth0IdAsync(
            auth0Id,
            User.FindFirst("email")?.Value ?? "",
            User.FindFirst("name")?.Value ?? auth0Id);

        var board = await _boardManager.CreateAsync(request, user.Id);
        return CreatedAtAction(nameof(GetById), new { id = board.Id }, board);
    }

    [HttpPost("{id:guid}/round")]
    [Authorize]
    public async Task<IActionResult> CreateRound(Guid id, [FromBody] CreateRoundRequest request)
    {
        var round = await _roundManager.CreateAsync(id, request);
        return Created($"/api/rounds/{round.Id}", round);
    }
}

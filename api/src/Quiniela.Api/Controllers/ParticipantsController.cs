using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quiniela.Api.DTOs.Participants;
using Quiniela.Api.Infrastructure;
using Quiniela.Api.Managers.Interfaces;

namespace Quiniela.Api.Controllers;

[ApiController]
public class ParticipantsController : ControllerBase
{
    private readonly IParticipantManager _participantManager;
    private readonly IUserManager _userManager;

    public ParticipantsController(IParticipantManager participantManager, IUserManager userManager)
    {
        _participantManager = participantManager;
        _userManager = userManager;
    }

    [HttpPost("api/round/{roundId:guid}/participate")]
    public async Task<IActionResult> Participate(Guid roundId, [FromBody] ParticipateRequest request)
    {
        Guid? userId = null;

        if (User.Identity?.IsAuthenticated == true)
        {
            var auth0Id = Auth0ClaimsHelper.GetAuth0Id(User);
            var user = await _userManager.GetOrCreateByAuth0IdAsync(
                auth0Id,
                User.FindFirst("email")?.Value ?? "",
                User.FindFirst("name")?.Value ?? auth0Id);
            userId = user.Id;
        }

        var participant = await _participantManager.ParticipateAsync(roundId, request, userId);
        return Created($"/api/participant/{participant.Id}", participant);
    }

    [HttpPut("api/participant/{id:guid}/activate")]
    [Authorize]
    [BoardAdminAuthorize(BoardResourceType.Participant)]
    public async Task<IActionResult> Activate(Guid id)
    {
        await _participantManager.ActivateAsync(id);
        return NoContent();
    }
}

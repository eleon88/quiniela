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

    [HttpPost("api/rounds/{roundId:guid}/participate")]
    public async Task<IActionResult> Participate(Guid roundId, [FromBody] ParticipateRequest request)
    {
        Guid? userId = null;

        if (User.Identity?.IsAuthenticated == true)
        {
            var auth0Id = Auth0ClaimsHelper.GetAuth0Id(User);
            var user = await _userManager.GetByAuth0IdAsync(auth0Id);
            userId = user?.Id;
        }

        var participant = await _participantManager.ParticipateAsync(roundId, request, userId);
        return Created($"/api/participants/{participant.Id}", participant);
    }

    [HttpPut("api/participants/{id:guid}/activate")]
    [Authorize]
    [BoardAdminAuthorize(BoardResourceType.Participant)]
    public async Task<IActionResult> Activate(Guid id)
    {
        await _participantManager.ActivateAsync(id);
        return NoContent();
    }
}

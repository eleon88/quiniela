using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quiniela.Api.DTOs.Users;
using Quiniela.Api.Infrastructure;
using Quiniela.Api.Managers.Interfaces;

namespace Quiniela.Api.Controllers;

[ApiController]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IUserManager _userManager;

    public UsersController(IUserManager userManager) => _userManager = userManager;

    [HttpPost("api/users/me")]
    public async Task<IActionResult> SyncCurrentUser([FromBody] SyncUserRequest request)
    {
        var auth0Id = Auth0ClaimsHelper.GetAuth0Id(User);
        await _userManager.SyncUserAsync(auth0Id, request.Email, request.DisplayName);
        return Ok();
    }
}

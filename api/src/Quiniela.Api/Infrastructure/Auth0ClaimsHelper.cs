using System.Security.Claims;

namespace Quiniela.Api.Infrastructure;

public static class Auth0ClaimsHelper
{
    public static string GetAuth0Id(ClaimsPrincipal user)
    {
        return user.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? throw new UnauthorizedAccessException("Missing Auth0 user ID in token.");
    }
}

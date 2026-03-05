using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Quiniela.Api.Enums;
using Quiniela.Api.Repositories.Interfaces;

namespace Quiniela.Api.Infrastructure;

/// <summary>
/// Specifies the type of resource identified by the route "id" parameter.
/// Used by BoardAdminAuthorizeAttribute to resolve the owning board.
/// </summary>
public enum BoardResourceType
{
    Board,
    Round,
    Match,
    Participant
}

/// <summary>
/// Authorization filter that verifies the current user has a BoardAdmin role
/// (Owner, Admin, or Moderator) for the board associated with the requested resource.
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class BoardAdminAuthorizeAttribute : TypeFilterAttribute
{
    public BoardAdminAuthorizeAttribute(BoardResourceType resourceType, string routeParam = "id")
        : base(typeof(BoardAdminAuthorizeFilter))
    {
        Arguments = [resourceType, routeParam];
    }
}

public class BoardAdminAuthorizeFilter : IAsyncAuthorizationFilter
{
    private readonly BoardResourceType _resourceType;
    private readonly string _routeParam;
    private readonly IUserRepository _userRepository;
    private readonly IBoardAdminRepository _boardAdminRepository;
    private readonly IRoundRepository _roundRepository;
    private readonly IMatchRepository _matchRepository;
    private readonly IParticipantRepository _participantRepository;

    public BoardAdminAuthorizeFilter(
        BoardResourceType resourceType,
        string routeParam,
        IUserRepository userRepository,
        IBoardAdminRepository boardAdminRepository,
        IRoundRepository roundRepository,
        IMatchRepository matchRepository,
        IParticipantRepository participantRepository)
    {
        _resourceType = resourceType;
        _routeParam = routeParam;
        _userRepository = userRepository;
        _boardAdminRepository = boardAdminRepository;
        _roundRepository = roundRepository;
        _matchRepository = matchRepository;
        _participantRepository = participantRepository;
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var user = context.HttpContext.User;
        if (user.Identity?.IsAuthenticated != true)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        if (!Guid.TryParse(context.RouteData.Values[_routeParam]?.ToString(), out var resourceId))
        {
            context.Result = new BadRequestObjectResult("Invalid resource ID.");
            return;
        }

        var auth0Id = Auth0ClaimsHelper.GetAuth0Id(user);
        var dbUser = await _userRepository.GetByAuth0IdAsync(auth0Id);
        if (dbUser == null)
        {
            context.Result = new ForbidResult();
            return;
        }

        var boardId = await ResolveBoardIdAsync(resourceId);
        if (boardId == null)
        {
            context.Result = new NotFoundResult();
            return;
        }

        var boardAdmin = await _boardAdminRepository.GetAsync(boardId.Value, dbUser.Id);
        if (boardAdmin == null)
        {
            context.Result = new ForbidResult();
            return;
        }
    }

    private async Task<Guid?> ResolveBoardIdAsync(Guid resourceId)
    {
        switch (_resourceType)
        {
            case BoardResourceType.Board:
                return resourceId;

            case BoardResourceType.Round:
                var round = await _roundRepository.GetByIdAsync(resourceId);
                return round?.BoardId;

            case BoardResourceType.Match:
                var match = await _matchRepository.GetByIdAsync(resourceId);
                if (match == null) return null;
                var matchRound = await _roundRepository.GetByIdAsync(match.RoundId);
                return matchRound?.BoardId;

            case BoardResourceType.Participant:
                var participant = await _participantRepository.GetByIdAsync(resourceId);
                if (participant == null) return null;
                var participantRound = await _roundRepository.GetByIdAsync(participant.RoundId);
                return participantRound?.BoardId;

            default:
                return null;
        }
    }
}

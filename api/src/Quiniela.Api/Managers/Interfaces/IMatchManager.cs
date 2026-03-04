using Quiniela.Api.DTOs.Matches;
using Quiniela.Api.Enums;

namespace Quiniela.Api.Managers.Interfaces;

public interface IMatchManager
{
    Task<IEnumerable<MatchResponse>> GetByRoundIdAsync(Guid roundId);
    Task<IEnumerable<MatchResponse>> CreateBatchAsync(Guid roundId, CreateMatchesRequest request);
    Task UpdateResultAsync(Guid matchId, MatchResult result);
}

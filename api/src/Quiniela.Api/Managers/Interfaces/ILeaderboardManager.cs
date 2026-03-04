using Quiniela.Api.DTOs.Leaderboard;

namespace Quiniela.Api.Managers.Interfaces;

public interface ILeaderboardManager
{
    Task<IEnumerable<LeaderboardEntryResponse>> GetLeaderboardAsync(Guid roundId);
}

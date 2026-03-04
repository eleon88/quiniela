using Quiniela.Api.DTOs.Leaderboard;
using Quiniela.Api.Managers.Interfaces;
using Quiniela.Api.Repositories.Interfaces;

namespace Quiniela.Api.Managers;

public class LeaderboardManager : ILeaderboardManager
{
    private readonly IParticipantRepository _participantRepo;

    public LeaderboardManager(IParticipantRepository participantRepo) => _participantRepo = participantRepo;

    public async Task<IEnumerable<LeaderboardEntryResponse>> GetLeaderboardAsync(Guid roundId)
    {
        var participants = await _participantRepo.GetByRoundIdOrderedAsync(roundId);
        var rank = 0;
        var lastScore = -1;
        var position = 0;

        return participants.Select(p =>
        {
            position++;
            if (p.Score != lastScore)
            {
                rank = position;
                lastScore = p.Score;
            }
            return new LeaderboardEntryResponse(rank, p.Id, p.DisplayName, p.Score);
        }).ToList();
    }
}

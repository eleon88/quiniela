namespace Quiniela.Api.DTOs.Leaderboard;

public record LeaderboardEntryResponse(
    int Rank,
    Guid ParticipantId,
    string DisplayName,
    int Score);

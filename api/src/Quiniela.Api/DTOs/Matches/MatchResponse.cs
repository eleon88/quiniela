using Quiniela.Api.Enums;

namespace Quiniela.Api.DTOs.Matches;

public record MatchResponse(
    Guid Id,
    Guid RoundId,
    string HomeTeam,
    string AwayTeam,
    DateTime StartDateTime,
    MatchResult Result,
    bool IsLocked);

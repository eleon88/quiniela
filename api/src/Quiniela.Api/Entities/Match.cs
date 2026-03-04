using Quiniela.Api.Enums;

namespace Quiniela.Api.Entities;

public class Match
{
    public Guid Id { get; set; }
    public Guid RoundId { get; set; }
    public string HomeTeam { get; set; } = string.Empty;
    public string AwayTeam { get; set; } = string.Empty;
    public DateTime StartDateTime { get; set; }
    public MatchResult Result { get; set; }
    public bool IsLocked { get; set; }
}

namespace Quiniela.Api.DTOs.Matches;

public record CreateMatchRequest(string HomeTeam, string AwayTeam, DateTime StartDateTime);

public record CreateMatchesRequest(List<CreateMatchRequest> Matches);

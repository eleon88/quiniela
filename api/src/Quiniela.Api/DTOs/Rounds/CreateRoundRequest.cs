namespace Quiniela.Api.DTOs.Rounds;

public record CreateRoundRequest(string Name, DateTime? StartDateTime, DateTime? EndDateTime);

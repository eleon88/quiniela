using Quiniela.Api.Enums;

namespace Quiniela.Api.DTOs.Rounds;

public record RoundResponse(
    Guid Id,
    Guid BoardId,
    string Name,
    RoundStatus Status,
    DateTime? StartDateTime,
    DateTime? EndDateTime,
    DateTime CreatedAt);

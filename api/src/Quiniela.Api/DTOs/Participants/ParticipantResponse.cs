namespace Quiniela.Api.DTOs.Participants;

public record ParticipantResponse(
    Guid Id,
    Guid RoundId,
    string DisplayName,
    bool IsActive,
    int Score,
    DateTime CreatedAt);

namespace Quiniela.Api.DTOs.Boards;

public record BoardResponse(
    Guid Id,
    string Name,
    string? Description,
    Guid OwnerUserId,
    bool IsPublic,
    bool IsPremium,
    DateTime CreatedAt);

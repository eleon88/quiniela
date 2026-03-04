namespace Quiniela.Api.DTOs.Boards;

public record CreateBoardRequest(string Name, string? Description, bool IsPublic);

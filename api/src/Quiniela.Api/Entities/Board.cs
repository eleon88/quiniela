namespace Quiniela.Api.Entities;

public class Board
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Guid OwnerUserId { get; set; }
    public bool IsPublic { get; set; }
    public bool IsPremium { get; set; }
    public DateTime CreatedAt { get; set; }
}

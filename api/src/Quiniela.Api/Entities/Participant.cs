namespace Quiniela.Api.Entities;

public class Participant
{
    public Guid Id { get; set; }
    public Guid RoundId { get; set; }
    public Guid? UserId { get; set; }
    public string DisplayName { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public int Score { get; set; }
    public DateTime CreatedAt { get; set; }
}

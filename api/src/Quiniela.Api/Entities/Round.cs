using Quiniela.Api.Enums;

namespace Quiniela.Api.Entities;

public class Round
{
    public Guid Id { get; set; }
    public Guid BoardId { get; set; }
    public string Name { get; set; } = string.Empty;
    public RoundStatus Status { get; set; }
    public DateTime? StartDateTime { get; set; }
    public DateTime? EndDateTime { get; set; }
    public DateTime CreatedAt { get; set; }
}

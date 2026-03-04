using Quiniela.Api.Enums;

namespace Quiniela.Api.Entities;

public class Prediction
{
    public Guid Id { get; set; }
    public Guid ParticipantId { get; set; }
    public Guid MatchId { get; set; }
    public SelectedOutcome SelectedOutcome { get; set; }
}

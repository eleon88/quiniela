using Quiniela.Api.Enums;

namespace Quiniela.Api.DTOs.Participants;

public record PredictionEntry(Guid MatchId, SelectedOutcome SelectedOutcome);

public record ParticipateRequest(string DisplayName, List<PredictionEntry> Predictions);

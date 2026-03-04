using Quiniela.Api.DTOs.Participants;

namespace Quiniela.Api.Managers.Interfaces;

public interface IParticipantManager
{
    Task<ParticipantResponse> ParticipateAsync(Guid roundId, ParticipateRequest request, Guid? userId);
    Task ActivateAsync(Guid participantId);
}

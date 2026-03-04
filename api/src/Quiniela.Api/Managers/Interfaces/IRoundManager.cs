using Quiniela.Api.DTOs.Rounds;
using Quiniela.Api.Enums;

namespace Quiniela.Api.Managers.Interfaces;

public interface IRoundManager
{
    Task<IEnumerable<RoundResponse>> GetByBoardIdAsync(Guid boardId);
    Task<RoundResponse> CreateAsync(Guid boardId, CreateRoundRequest request);
    Task UpdateStatusAsync(Guid roundId, RoundStatus newStatus);
}

using System.Data;
using Quiniela.Api.Entities;

namespace Quiniela.Api.Repositories.Interfaces;

public interface IParticipantRepository
{
    Task<Participant?> GetByIdAsync(Guid id);
    Task<IEnumerable<Participant>> GetByRoundIdOrderedAsync(Guid roundId);
    Task<Participant> CreateAsync(Participant participant, IDbConnection? connection = null, IDbTransaction? transaction = null);
    Task ActivateAsync(Guid id, IDbConnection? connection = null, IDbTransaction? transaction = null);
    Task RecalculateScoresAsync(Guid roundId, IDbConnection? connection = null, IDbTransaction? transaction = null);
}

using System.Data;
using Quiniela.Api.Entities;
using Quiniela.Api.Enums;

namespace Quiniela.Api.Repositories.Interfaces;

public interface IRoundRepository
{
    Task<IEnumerable<Round>> GetByBoardIdAsync(Guid boardId);
    Task<Round?> GetByIdAsync(Guid id);
    Task<Round> CreateAsync(Round round, IDbConnection? connection = null, IDbTransaction? transaction = null);
    Task UpdateStatusAsync(Guid id, RoundStatus status, IDbConnection? connection = null, IDbTransaction? transaction = null);
}

using System.Data;
using Quiniela.Api.Entities;
using Quiniela.Api.Enums;

namespace Quiniela.Api.Repositories.Interfaces;

public interface IMatchRepository
{
    Task<IEnumerable<Match>> GetByRoundIdAsync(Guid roundId);
    Task<Match?> GetByIdAsync(Guid id);
    Task CreateBatchAsync(IEnumerable<Match> matches, IDbConnection? connection = null, IDbTransaction? transaction = null);
    Task UpdateResultAsync(Guid id, MatchResult result, IDbConnection? connection = null, IDbTransaction? transaction = null);
}

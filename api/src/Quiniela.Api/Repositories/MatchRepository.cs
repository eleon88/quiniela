using System.Data;
using Dapper;
using Quiniela.Api.Entities;
using Quiniela.Api.Enums;
using Quiniela.Api.Infrastructure;
using Quiniela.Api.Repositories.Interfaces;

namespace Quiniela.Api.Repositories;

public class MatchRepository : IMatchRepository
{
    private readonly IDbConnectionFactory _db;

    public MatchRepository(IDbConnectionFactory db) => _db = db;

    public async Task<IEnumerable<Match>> GetByRoundIdAsync(Guid roundId)
    {
        using var conn = _db.CreateConnection();
        return await conn.QueryAsync<Match>(
            "dbo.usp_GetMatchesByRoundId",
            new { RoundId = roundId },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<Match?> GetByIdAsync(Guid id)
    {
        using var conn = _db.CreateConnection();
        return await conn.QuerySingleOrDefaultAsync<Match>(
            "dbo.usp_GetMatchById",
            new { Id = id },
            commandType: CommandType.StoredProcedure);
    }

    public async Task CreateBatchAsync(IEnumerable<Match> matches, IDbConnection? connection = null, IDbTransaction? transaction = null)
    {
        var conn = connection ?? _db.CreateConnection();
        try
        {
            foreach (var match in matches)
            {
                match.Id = Guid.NewGuid();
                await conn.ExecuteAsync(
                    "dbo.usp_CreateMatch",
                    new { match.Id, match.RoundId, match.HomeTeam, match.AwayTeam, match.StartDateTime, match.Result, match.IsLocked },
                    transaction,
                    commandType: CommandType.StoredProcedure);
            }
        }
        finally
        {
            if (connection == null) conn.Dispose();
        }
    }

    public async Task UpdateResultAsync(Guid id, MatchResult result, IDbConnection? connection = null, IDbTransaction? transaction = null)
    {
        var conn = connection ?? _db.CreateConnection();
        try
        {
            await conn.ExecuteAsync(
                "dbo.usp_UpdateMatchResult",
                new { Id = id, Result = result },
                transaction,
                commandType: CommandType.StoredProcedure);
        }
        finally
        {
            if (connection == null) conn.Dispose();
        }
    }
}

using System.Data;
using Dapper;
using Quiniela.Api.Entities;
using Quiniela.Api.Enums;
using Quiniela.Api.Infrastructure;
using Quiniela.Api.Repositories.Interfaces;

namespace Quiniela.Api.Repositories;

public class RoundRepository : IRoundRepository
{
    private readonly IDbConnectionFactory _db;

    public RoundRepository(IDbConnectionFactory db) => _db = db;

    public async Task<IEnumerable<Round>> GetByBoardIdAsync(Guid boardId)
    {
        using var conn = _db.CreateConnection();
        return await conn.QueryAsync<Round>(
            "dbo.usp_GetRoundsByBoardId",
            new { BoardId = boardId },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<Round?> GetByIdAsync(Guid id)
    {
        using var conn = _db.CreateConnection();
        return await conn.QuerySingleOrDefaultAsync<Round>(
            "dbo.usp_GetRoundById",
            new { Id = id },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<Round> CreateAsync(Round round, IDbConnection? connection = null, IDbTransaction? transaction = null)
    {
        round.Id = Guid.NewGuid();
        var conn = connection ?? _db.CreateConnection();
        try
        {
            await conn.ExecuteAsync(
                "dbo.usp_CreateRound",
                new { round.Id, round.BoardId, round.Name, round.Status, round.StartDateTime, round.EndDateTime },
                transaction,
                commandType: CommandType.StoredProcedure);
            return round;
        }
        finally
        {
            if (connection == null) conn.Dispose();
        }
    }

    public async Task UpdateStatusAsync(Guid id, RoundStatus status, IDbConnection? connection = null, IDbTransaction? transaction = null)
    {
        var conn = connection ?? _db.CreateConnection();
        try
        {
            await conn.ExecuteAsync(
                "dbo.usp_UpdateRoundStatus",
                new { Id = id, Status = status },
                transaction,
                commandType: CommandType.StoredProcedure);
        }
        finally
        {
            if (connection == null) conn.Dispose();
        }
    }
}

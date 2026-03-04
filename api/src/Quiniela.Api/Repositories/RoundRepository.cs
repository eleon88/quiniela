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
            "SELECT * FROM Round WHERE BoardId = @BoardId ORDER BY CreatedAt DESC",
            new { BoardId = boardId });
    }

    public async Task<Round?> GetByIdAsync(Guid id)
    {
        using var conn = _db.CreateConnection();
        return await conn.QuerySingleOrDefaultAsync<Round>(
            "SELECT * FROM Round WHERE Id = @Id", new { Id = id });
    }

    public async Task<Round> CreateAsync(Round round, IDbConnection? connection = null, IDbTransaction? transaction = null)
    {
        round.Id = Guid.NewGuid();
        var conn = connection ?? _db.CreateConnection();
        try
        {
            await conn.ExecuteAsync(
                @"INSERT INTO Round (Id, BoardId, Name, Status, StartDateTime, EndDateTime)
                  VALUES (@Id, @BoardId, @Name, @Status, @StartDateTime, @EndDateTime)",
                round, transaction);
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
                "UPDATE Round SET Status = @Status WHERE Id = @Id",
                new { Id = id, Status = status }, transaction);
        }
        finally
        {
            if (connection == null) conn.Dispose();
        }
    }
}

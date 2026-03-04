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
            "SELECT * FROM Match WHERE RoundId = @RoundId ORDER BY StartDateTime",
            new { RoundId = roundId });
    }

    public async Task<Match?> GetByIdAsync(Guid id)
    {
        using var conn = _db.CreateConnection();
        return await conn.QuerySingleOrDefaultAsync<Match>(
            "SELECT * FROM Match WHERE Id = @Id", new { Id = id });
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
                    @"INSERT INTO Match (Id, RoundId, HomeTeam, AwayTeam, StartDateTime, Result, IsLocked)
                      VALUES (@Id, @RoundId, @HomeTeam, @AwayTeam, @StartDateTime, @Result, @IsLocked)",
                    match, transaction);
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
                "UPDATE Match SET Result = @Result, IsLocked = 1 WHERE Id = @Id",
                new { Id = id, Result = result }, transaction);
        }
        finally
        {
            if (connection == null) conn.Dispose();
        }
    }
}

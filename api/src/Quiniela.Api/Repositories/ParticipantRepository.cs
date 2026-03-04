using System.Data;
using Dapper;
using Quiniela.Api.Entities;
using Quiniela.Api.Infrastructure;
using Quiniela.Api.Repositories.Interfaces;

namespace Quiniela.Api.Repositories;

public class ParticipantRepository : IParticipantRepository
{
    private readonly IDbConnectionFactory _db;

    public ParticipantRepository(IDbConnectionFactory db) => _db = db;

    public async Task<Participant?> GetByIdAsync(Guid id)
    {
        using var conn = _db.CreateConnection();
        return await conn.QuerySingleOrDefaultAsync<Participant>(
            "SELECT * FROM Participant WHERE Id = @Id", new { Id = id });
    }

    public async Task<IEnumerable<Participant>> GetByRoundIdOrderedAsync(Guid roundId)
    {
        using var conn = _db.CreateConnection();
        return await conn.QueryAsync<Participant>(
            @"SELECT * FROM Participant
              WHERE RoundId = @RoundId AND IsActive = 1
              ORDER BY Score DESC, DisplayName ASC",
            new { RoundId = roundId });
    }

    public async Task<Participant> CreateAsync(Participant participant, IDbConnection? connection = null, IDbTransaction? transaction = null)
    {
        participant.Id = Guid.NewGuid();
        var conn = connection ?? _db.CreateConnection();
        try
        {
            await conn.ExecuteAsync(
                @"INSERT INTO Participant (Id, RoundId, UserId, DisplayName, IsActive, Score)
                  VALUES (@Id, @RoundId, @UserId, @DisplayName, @IsActive, @Score)",
                participant, transaction);
            return participant;
        }
        finally
        {
            if (connection == null) conn.Dispose();
        }
    }

    public async Task ActivateAsync(Guid id, IDbConnection? connection = null, IDbTransaction? transaction = null)
    {
        var conn = connection ?? _db.CreateConnection();
        try
        {
            await conn.ExecuteAsync(
                "UPDATE Participant SET IsActive = 1 WHERE Id = @Id",
                new { Id = id }, transaction);
        }
        finally
        {
            if (connection == null) conn.Dispose();
        }
    }

    public async Task RecalculateScoresAsync(Guid roundId, IDbConnection? connection = null, IDbTransaction? transaction = null)
    {
        var conn = connection ?? _db.CreateConnection();
        try
        {
            await conn.ExecuteAsync(
                @"UPDATE p SET p.Score = (
                    SELECT COUNT(*)
                    FROM Prediction pred
                    INNER JOIN Match m ON pred.MatchId = m.Id
                    WHERE pred.ParticipantId = p.Id
                      AND m.Result <> 3
                      AND CAST(pred.SelectedOutcome AS TINYINT) = CAST(m.Result AS TINYINT)
                  )
                  FROM Participant p
                  WHERE p.RoundId = @RoundId AND p.IsActive = 1",
                new { RoundId = roundId }, transaction);
        }
        finally
        {
            if (connection == null) conn.Dispose();
        }
    }
}

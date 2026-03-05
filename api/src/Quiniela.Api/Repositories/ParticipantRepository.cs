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
            "dbo.usp_GetParticipantById",
            new { Id = id },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<IEnumerable<Participant>> GetByRoundIdOrderedAsync(Guid roundId)
    {
        using var conn = _db.CreateConnection();
        return await conn.QueryAsync<Participant>(
            "dbo.usp_GetActiveParticipantsByRoundId",
            new { RoundId = roundId },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<Participant> CreateAsync(Participant participant, IDbConnection? connection = null, IDbTransaction? transaction = null)
    {
        participant.Id = Guid.NewGuid();
        var conn = connection ?? _db.CreateConnection();
        try
        {
            await conn.ExecuteAsync(
                "dbo.usp_CreateParticipant",
                new { participant.Id, participant.RoundId, participant.UserId, participant.DisplayName, participant.IsActive, participant.Score },
                transaction,
                commandType: CommandType.StoredProcedure);
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
                "dbo.usp_ActivateParticipant",
                new { Id = id },
                transaction,
                commandType: CommandType.StoredProcedure);
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
                "dbo.usp_RecalculateScoresByRoundId",
                new { RoundId = roundId },
                transaction,
                commandType: CommandType.StoredProcedure);
        }
        finally
        {
            if (connection == null) conn.Dispose();
        }
    }
}

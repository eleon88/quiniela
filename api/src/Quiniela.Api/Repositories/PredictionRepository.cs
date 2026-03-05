using System.Data;
using Dapper;
using Quiniela.Api.Entities;
using Quiniela.Api.Infrastructure;
using Quiniela.Api.Repositories.Interfaces;

namespace Quiniela.Api.Repositories;

public class PredictionRepository : IPredictionRepository
{
    private readonly IDbConnectionFactory _db;

    public PredictionRepository(IDbConnectionFactory db) => _db = db;

    public async Task CreateBatchAsync(IEnumerable<Prediction> predictions, IDbConnection? connection = null, IDbTransaction? transaction = null)
    {
        var conn = connection ?? _db.CreateConnection();
        try
        {
            foreach (var prediction in predictions)
            {
                prediction.Id = Guid.NewGuid();
                await conn.ExecuteAsync(
                    "dbo.usp_CreatePrediction",
                    new { prediction.Id, prediction.ParticipantId, prediction.MatchId, prediction.SelectedOutcome },
                    transaction,
                    commandType: CommandType.StoredProcedure);
            }
        }
        finally
        {
            if (connection == null) conn.Dispose();
        }
    }
}

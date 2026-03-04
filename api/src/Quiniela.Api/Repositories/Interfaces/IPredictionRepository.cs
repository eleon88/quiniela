using System.Data;
using Quiniela.Api.Entities;

namespace Quiniela.Api.Repositories.Interfaces;

public interface IPredictionRepository
{
    Task CreateBatchAsync(IEnumerable<Prediction> predictions, IDbConnection? connection = null, IDbTransaction? transaction = null);
}

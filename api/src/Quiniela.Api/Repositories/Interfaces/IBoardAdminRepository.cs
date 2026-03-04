using System.Data;
using Quiniela.Api.Entities;

namespace Quiniela.Api.Repositories.Interfaces;

public interface IBoardAdminRepository
{
    Task<BoardAdmin?> GetAsync(Guid boardId, Guid userId);
    Task CreateAsync(BoardAdmin boardAdmin, IDbConnection? connection = null, IDbTransaction? transaction = null);
}

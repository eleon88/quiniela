using System.Data;
using Quiniela.Api.Entities;

namespace Quiniela.Api.Repositories.Interfaces;

public interface IBoardRepository
{
    Task<IEnumerable<Board>> GetPublicBoardsAsync();
    Task<Board?> GetByIdAsync(Guid id);
    Task<Board> CreateAsync(Board board, IDbConnection? connection = null, IDbTransaction? transaction = null);
    Task<IEnumerable<Board>> GetBoardsByAdminAsync(Guid userId);
}

using System.Data;
using Dapper;
using Quiniela.Api.Entities;
using Quiniela.Api.Infrastructure;
using Quiniela.Api.Repositories.Interfaces;

namespace Quiniela.Api.Repositories;

public class BoardRepository : IBoardRepository
{
    private readonly IDbConnectionFactory _db;

    public BoardRepository(IDbConnectionFactory db) => _db = db;

    public async Task<IEnumerable<Board>> GetPublicBoardsAsync()
    {
        using var conn = _db.CreateConnection();
        return await conn.QueryAsync<Board>(
            "dbo.usp_GetPublicBoards",
            commandType: CommandType.StoredProcedure);
    }

    public async Task<Board?> GetByIdAsync(Guid id)
    {
        using var conn = _db.CreateConnection();
        return await conn.QuerySingleOrDefaultAsync<Board>(
            "dbo.usp_GetBoardById",
            new { Id = id },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<Board> CreateAsync(Board board, IDbConnection? connection = null, IDbTransaction? transaction = null)
    {
        board.Id = Guid.NewGuid();
        var conn = connection ?? _db.CreateConnection();
        try
        {
            await conn.ExecuteAsync(
                "dbo.usp_CreateBoard",
                new { board.Id, board.Name, board.Description, board.OwnerUserId, board.IsPublic, board.IsPremium },
                transaction,
                commandType: CommandType.StoredProcedure);
            return board;
        }
        finally
        {
            if (connection == null) conn.Dispose();
        }
    }
}

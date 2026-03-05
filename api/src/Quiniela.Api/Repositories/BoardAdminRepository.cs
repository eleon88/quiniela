using System.Data;
using Dapper;
using Quiniela.Api.Entities;
using Quiniela.Api.Infrastructure;
using Quiniela.Api.Repositories.Interfaces;

namespace Quiniela.Api.Repositories;

public class BoardAdminRepository : IBoardAdminRepository
{
    private readonly IDbConnectionFactory _db;

    public BoardAdminRepository(IDbConnectionFactory db) => _db = db;

    public async Task<BoardAdmin?> GetAsync(Guid boardId, Guid userId)
    {
        using var conn = _db.CreateConnection();
        return await conn.QuerySingleOrDefaultAsync<BoardAdmin>(
            "dbo.usp_GetBoardAdmin",
            new { BoardId = boardId, UserId = userId },
            commandType: CommandType.StoredProcedure);
    }

    public async Task CreateAsync(BoardAdmin boardAdmin, IDbConnection? connection = null, IDbTransaction? transaction = null)
    {
        var conn = connection ?? _db.CreateConnection();
        try
        {
            await conn.ExecuteAsync(
                "dbo.usp_CreateBoardAdmin",
                new { boardAdmin.BoardId, boardAdmin.UserId, boardAdmin.Role },
                transaction,
                commandType: CommandType.StoredProcedure);
        }
        finally
        {
            if (connection == null) conn.Dispose();
        }
    }
}

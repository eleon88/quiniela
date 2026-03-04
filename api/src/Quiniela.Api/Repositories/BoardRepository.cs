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
            "SELECT * FROM Board WHERE IsPublic = 1 ORDER BY CreatedAt DESC");
    }

    public async Task<Board?> GetByIdAsync(Guid id)
    {
        using var conn = _db.CreateConnection();
        return await conn.QuerySingleOrDefaultAsync<Board>(
            "SELECT * FROM Board WHERE Id = @Id", new { Id = id });
    }

    public async Task<Board> CreateAsync(Board board, IDbConnection? connection = null, IDbTransaction? transaction = null)
    {
        board.Id = Guid.NewGuid();
        var conn = connection ?? _db.CreateConnection();
        try
        {
            await conn.ExecuteAsync(
                @"INSERT INTO Board (Id, Name, Description, OwnerUserId, IsPublic, IsPremium)
                  VALUES (@Id, @Name, @Description, @OwnerUserId, @IsPublic, @IsPremium)",
                board, transaction);
            return board;
        }
        finally
        {
            if (connection == null) conn.Dispose();
        }
    }
}

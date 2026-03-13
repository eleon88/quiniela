using Quiniela.Api.DTOs.Boards;
using Quiniela.Api.Entities;
using Quiniela.Api.Enums;
using Quiniela.Api.Infrastructure;
using Quiniela.Api.Managers.Interfaces;
using Quiniela.Api.Repositories.Interfaces;

namespace Quiniela.Api.Managers;

public class BoardManager : IBoardManager
{
    private readonly IBoardRepository _boardRepo;
    private readonly IBoardAdminRepository _boardAdminRepo;
    private readonly IDbConnectionFactory _db;

    public BoardManager(IBoardRepository boardRepo, IBoardAdminRepository boardAdminRepo, IDbConnectionFactory db)
    {
        _boardRepo = boardRepo;
        _boardAdminRepo = boardAdminRepo;
        _db = db;
    }

    public async Task<IEnumerable<BoardResponse>> GetPublicBoardsAsync()
    {
        var boards = await _boardRepo.GetPublicBoardsAsync();
        return boards.Select(MapToResponse);
    }

    public async Task<BoardResponse> GetByIdAsync(Guid id)
    {
        var board = await _boardRepo.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Board {id} not found.");
        return MapToResponse(board);
    }

    public async Task<BoardResponse> CreateAsync(CreateBoardRequest request, Guid ownerUserId)
    {
        using var conn = _db.CreateConnection();
        conn.Open();
        using var tx = conn.BeginTransaction();

        var board = await _boardRepo.CreateAsync(new Board
        {
            Name = request.Name,
            Description = request.Description,
            OwnerUserId = ownerUserId,
            IsPublic = request.IsPublic
        }, conn, tx);

        await _boardAdminRepo.CreateAsync(new BoardAdmin
        {
            BoardId = board.Id,
            UserId = ownerUserId,
            Role = BoardAdminRole.Owner
        }, conn, tx);

        tx.Commit();
        return MapToResponse(board);
    }

    public async Task<IEnumerable<BoardResponse>> GetBoardsByAdminAsync(Guid userId)
    {
        var boards = await _boardRepo.GetBoardsByAdminAsync(userId);
        return boards.Select(MapToResponse);
    }

    private static BoardResponse MapToResponse(Board b) =>
        new(b.Id, b.Name, b.Description, b.OwnerUserId, b.IsPublic, b.IsPremium, b.CreatedAt);
}

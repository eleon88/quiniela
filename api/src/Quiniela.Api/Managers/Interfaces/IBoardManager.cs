using Quiniela.Api.DTOs.Boards;

namespace Quiniela.Api.Managers.Interfaces;

public interface IBoardManager
{
    Task<IEnumerable<BoardResponse>> GetPublicBoardsAsync();
    Task<BoardResponse> GetByIdAsync(Guid id);
    Task<BoardResponse> CreateAsync(CreateBoardRequest request, Guid ownerUserId);
}

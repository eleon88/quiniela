using Microsoft.AspNetCore.SignalR;
using Quiniela.Api.DTOs.Rounds;
using Quiniela.Api.Entities;
using Quiniela.Api.Enums;
using Quiniela.Api.Hubs;
using Quiniela.Api.Managers.Interfaces;
using Quiniela.Api.Repositories.Interfaces;

namespace Quiniela.Api.Managers;

public class RoundManager : IRoundManager
{
    private readonly IRoundRepository _roundRepo;
    private readonly IHubContext<LeaderboardHub> _hubContext;

    public RoundManager(IRoundRepository roundRepo, IHubContext<LeaderboardHub> hubContext)
    {
        _roundRepo = roundRepo;
        _hubContext = hubContext;
    }

    public async Task<IEnumerable<RoundResponse>> GetByBoardIdAsync(Guid boardId)
    {
        var rounds = await _roundRepo.GetByBoardIdAsync(boardId);
        return rounds.Select(MapToResponse);
    }

    public async Task<RoundResponse?> GetByIdAsync(Guid id)
    {
        var round = await _roundRepo.GetByIdAsync(id);
        return round is null ? null : MapToResponse(round);
    }

    public async Task<RoundResponse> CreateAsync(Guid boardId, CreateRoundRequest request)
    {
        var round = await _roundRepo.CreateAsync(new Round
        {
            BoardId = boardId,
            Name = request.Name,
            Status = RoundStatus.Draft,
            StartDateTime = request.StartDateTime,
            EndDateTime = request.EndDateTime
        });
        return MapToResponse(round);
    }

    public async Task UpdateStatusAsync(Guid roundId, RoundStatus newStatus)
    {
        var round = await _roundRepo.GetByIdAsync(roundId)
            ?? throw new KeyNotFoundException($"Round {roundId} not found.");

        ValidateTransition(round.Status, newStatus);
        await _roundRepo.UpdateStatusAsync(roundId, newStatus);

        await _hubContext.Clients.Group(roundId.ToString())
            .SendAsync("RoundStatusChanged", new { RoundId = roundId, Status = newStatus });
    }

    private static void ValidateTransition(RoundStatus current, RoundStatus next)
    {
        var valid = (current, next) switch
        {
            (RoundStatus.Draft, RoundStatus.Open) => true,
            (RoundStatus.Open, RoundStatus.Active) => true,
            (RoundStatus.Active, RoundStatus.Completed) => true,
            _ => false
        };

        if (!valid)
            throw new InvalidOperationException($"Cannot transition from {current} to {next}.");
    }

    private static RoundResponse MapToResponse(Round r) =>
        new(r.Id, r.BoardId, r.Name, r.Status, r.StartDateTime, r.EndDateTime, r.CreatedAt);
}

using Microsoft.AspNetCore.SignalR;
using Quiniela.Api.DTOs.Matches;
using Quiniela.Api.Entities;
using Quiniela.Api.Enums;
using Quiniela.Api.Hubs;
using Quiniela.Api.Infrastructure;
using Quiniela.Api.Managers.Interfaces;
using Quiniela.Api.Repositories.Interfaces;

namespace Quiniela.Api.Managers;

public class MatchManager : IMatchManager
{
    private readonly IMatchRepository _matchRepo;
    private readonly IParticipantRepository _participantRepo;
    private readonly IDbConnectionFactory _db;
    private readonly IHubContext<LeaderboardHub> _hubContext;
    private readonly ILeaderboardManager _leaderboardManager;

    public MatchManager(
        IMatchRepository matchRepo,
        IParticipantRepository participantRepo,
        IDbConnectionFactory db,
        IHubContext<LeaderboardHub> hubContext,
        ILeaderboardManager leaderboardManager)
    {
        _matchRepo = matchRepo;
        _participantRepo = participantRepo;
        _db = db;
        _hubContext = hubContext;
        _leaderboardManager = leaderboardManager;
    }

    public async Task<IEnumerable<MatchResponse>> GetByRoundIdAsync(Guid roundId)
    {
        var matches = await _matchRepo.GetByRoundIdAsync(roundId);
        return matches.Select(MapToResponse);
    }

    public async Task<IEnumerable<MatchResponse>> CreateBatchAsync(Guid roundId, CreateMatchesRequest request)
    {
        var matches = request.Matches.Select(m => new Match
        {
            RoundId = roundId,
            HomeTeam = m.HomeTeam,
            AwayTeam = m.AwayTeam,
            StartDateTime = m.StartDateTime,
            Result = MatchResult.Pending,
            IsLocked = false
        }).ToList();

        await _matchRepo.CreateBatchAsync(matches);
        return matches.Select(MapToResponse);
    }

    public async Task UpdateResultAsync(Guid matchId, MatchResult result)
    {
        var match = await _matchRepo.GetByIdAsync(matchId)
            ?? throw new KeyNotFoundException($"Match {matchId} not found.");

        if (result == MatchResult.Pending)
            throw new InvalidOperationException("Cannot set result to Pending.");

        using var conn = _db.CreateConnection();
        conn.Open();
        using var tx = conn.BeginTransaction();

        await _matchRepo.UpdateResultAsync(matchId, result, conn, tx);
        await _participantRepo.RecalculateScoresAsync(match.RoundId, conn, tx);

        tx.Commit();

        // Broadcast after commit
        var roundId = match.RoundId;
        var leaderboard = await _leaderboardManager.GetLeaderboardAsync(roundId);

        await _hubContext.Clients.Group(roundId.ToString())
            .SendAsync("MatchResultUpdated", new { MatchId = matchId, Result = result });
        await _hubContext.Clients.Group(roundId.ToString())
            .SendAsync("LeaderboardUpdated", leaderboard);
    }

    private static MatchResponse MapToResponse(Match m) =>
        new(m.Id, m.RoundId, m.HomeTeam, m.AwayTeam, m.StartDateTime, m.Result, m.IsLocked);
}

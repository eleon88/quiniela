using Quiniela.Api.DTOs.Participants;
using Quiniela.Api.Entities;
using Quiniela.Api.Enums;
using Quiniela.Api.Infrastructure;
using Quiniela.Api.Managers.Interfaces;
using Quiniela.Api.Repositories.Interfaces;

namespace Quiniela.Api.Managers;

public class ParticipantManager : IParticipantManager
{
    private readonly IParticipantRepository _participantRepo;
    private readonly IPredictionRepository _predictionRepo;
    private readonly IRoundRepository _roundRepo;
    private readonly IDbConnectionFactory _db;

    public ParticipantManager(
        IParticipantRepository participantRepo,
        IPredictionRepository predictionRepo,
        IRoundRepository roundRepo,
        IDbConnectionFactory db)
    {
        _participantRepo = participantRepo;
        _predictionRepo = predictionRepo;
        _roundRepo = roundRepo;
        _db = db;
    }

    public async Task<ParticipantResponse> ParticipateAsync(Guid roundId, ParticipateRequest request, Guid? userId)
    {
        var round = await _roundRepo.GetByIdAsync(roundId)
            ?? throw new KeyNotFoundException($"Round {roundId} not found.");

        if (round.Status != RoundStatus.Open)
            throw new InvalidOperationException("Round is not open for submissions.");

        using var conn = _db.CreateConnection();
        conn.Open();
        using var tx = conn.BeginTransaction();

        var participant = await _participantRepo.CreateAsync(new Participant
        {
            RoundId = roundId,
            UserId = userId,
            DisplayName = request.DisplayName,
            IsActive = false,
            Score = 0
        }, conn, tx);

        var predictions = request.Predictions.Select(p => new Prediction
        {
            ParticipantId = participant.Id,
            MatchId = p.MatchId,
            SelectedOutcome = p.SelectedOutcome
        });

        await _predictionRepo.CreateBatchAsync(predictions, conn, tx);

        tx.Commit();

        return new ParticipantResponse(
            participant.Id, participant.RoundId, participant.DisplayName,
            participant.IsActive, participant.Score, participant.CreatedAt);
    }

    public async Task ActivateAsync(Guid participantId)
    {
        var participant = await _participantRepo.GetByIdAsync(participantId)
            ?? throw new KeyNotFoundException($"Participant {participantId} not found.");

        if (participant.IsActive)
            throw new InvalidOperationException("Participant is already active.");

        await _participantRepo.ActivateAsync(participantId);
    }
}

CREATE OR ALTER PROCEDURE [dbo].[usp_CreatePrediction]
    @Id UNIQUEIDENTIFIER,
    @ParticipantId UNIQUEIDENTIFIER,
    @MatchId UNIQUEIDENTIFIER,
    @SelectedOutcome TINYINT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO [dbo].[Prediction] (Id, ParticipantId, MatchId, SelectedOutcome)
    VALUES (@Id, @ParticipantId, @MatchId, @SelectedOutcome);
END

CREATE OR ALTER PROCEDURE [dbo].[usp_RecalculateScoresByRoundId]
    @RoundId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE p
    SET
        p.Score = (
            SELECT COUNT(*)
            FROM [dbo].[Prediction] pred
            INNER JOIN [dbo].[Match] m ON pred.MatchId = m.Id
            WHERE
                pred.ParticipantId = p.Id
                AND m.Result <> 3
                AND CAST(pred.SelectedOutcome AS TINYINT) = CAST(m.Result AS TINYINT)
        )
    FROM
        [dbo].[Participant] p
    WHERE
        p.RoundId = @RoundId
        AND p.IsActive = 1;
END

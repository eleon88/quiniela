CREATE OR ALTER PROCEDURE [dbo].[usp_GetActiveParticipantsByRoundId]
    @RoundId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        Id,
        RoundId,
        UserId,
        DisplayName,
        IsActive,
        Score,
        CreatedAt
    FROM
        [dbo].[Participant]
    WHERE
        RoundId = @RoundId
        AND IsActive = 1
    ORDER BY
        Score DESC,
        DisplayName ASC;
END

CREATE OR ALTER PROCEDURE [dbo].[usp_GetMatchesByRoundId]
    @RoundId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        Id,
        RoundId,
        HomeTeam,
        AwayTeam,
        StartDateTime,
        Result,
        IsLocked
    FROM
        [dbo].[Match]
    WHERE
        RoundId = @RoundId
    ORDER BY
        StartDateTime;
END

CREATE OR ALTER PROCEDURE [dbo].[usp_GetMatchById]
    @Id UNIQUEIDENTIFIER
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
        Id = @Id;
END

CREATE OR ALTER PROCEDURE [dbo].[usp_GetParticipantById]
    @Id UNIQUEIDENTIFIER
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
        Id = @Id;
END

CREATE OR ALTER PROCEDURE [dbo].[usp_GetRoundById]
    @Id UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        Id,
        BoardId,
        Name,
        Status,
        StartDateTime,
        EndDateTime,
        CreatedAt
    FROM
        [dbo].[Round]
    WHERE
        Id = @Id;
END

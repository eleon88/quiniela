CREATE OR ALTER PROCEDURE [dbo].[usp_GetRoundsByBoardId]
    @BoardId UNIQUEIDENTIFIER
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
        BoardId = @BoardId
    ORDER BY
        CreatedAt DESC;
END

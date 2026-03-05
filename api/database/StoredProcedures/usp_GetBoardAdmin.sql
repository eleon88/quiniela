CREATE OR ALTER PROCEDURE [dbo].[usp_GetBoardAdmin]
    @BoardId UNIQUEIDENTIFIER,
    @UserId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        BoardId,
        UserId,
        Role
    FROM
        [dbo].[BoardAdmin]
    WHERE
        BoardId = @BoardId
        AND UserId = @UserId;
END

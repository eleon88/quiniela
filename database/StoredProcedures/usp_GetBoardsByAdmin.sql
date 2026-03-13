CREATE OR ALTER PROCEDURE dbo.usp_GetBoardsByAdmin
    @UserId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        b.Id,
        b.Name,
        b.Description,
        b.OwnerUserId,
        b.IsPublic,
        b.IsPremium,
        b.CreatedAt
    FROM dbo.Board b
    INNER JOIN dbo.BoardAdmin ba ON ba.BoardId = b.Id
    WHERE ba.UserId = @UserId
    ORDER BY b.CreatedAt DESC;
END;

CREATE OR ALTER PROCEDURE [dbo].[usp_GetPublicBoards]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        Id,
        Name,
        Description,
        OwnerUserId,
        IsPublic,
        IsPremium,
        CreatedAt
    FROM
        [dbo].[Board]
    WHERE
        IsPublic = 1
    ORDER BY
        CreatedAt DESC;
END

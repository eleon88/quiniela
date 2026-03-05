CREATE OR ALTER PROCEDURE [dbo].[usp_GetBoardById]
    @Id UNIQUEIDENTIFIER
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
        Id = @Id;
END

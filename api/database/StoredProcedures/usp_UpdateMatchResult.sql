CREATE OR ALTER PROCEDURE [dbo].[usp_UpdateMatchResult]
    @Id UNIQUEIDENTIFIER,
    @Result TINYINT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE [dbo].[Match]
    SET
        Result = @Result,
        IsLocked = 1
    WHERE
        Id = @Id;
END

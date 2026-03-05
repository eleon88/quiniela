CREATE OR ALTER PROCEDURE [dbo].[usp_CreateBoardAdmin]
    @BoardId UNIQUEIDENTIFIER,
    @UserId UNIQUEIDENTIFIER,
    @Role TINYINT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO [dbo].[BoardAdmin] (BoardId, UserId, Role)
    VALUES (@BoardId, @UserId, @Role);
END

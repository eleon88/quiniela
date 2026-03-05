CREATE OR ALTER PROCEDURE [dbo].[usp_CreateRound]
    @Id UNIQUEIDENTIFIER,
    @BoardId UNIQUEIDENTIFIER,
    @Name NVARCHAR(255),
    @Status TINYINT,
    @StartDateTime DATETIME2,
    @EndDateTime DATETIME2
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO [dbo].[Round] (Id, BoardId, Name, Status, StartDateTime, EndDateTime)
    VALUES (@Id, @BoardId, @Name, @Status, @StartDateTime, @EndDateTime);
END

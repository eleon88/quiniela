CREATE OR ALTER PROCEDURE [dbo].[usp_CreateParticipant]
    @Id UNIQUEIDENTIFIER,
    @RoundId UNIQUEIDENTIFIER,
    @UserId UNIQUEIDENTIFIER = NULL,
    @DisplayName NVARCHAR(255),
    @IsActive BIT,
    @Score INT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO [dbo].[Participant] (Id, RoundId, UserId, DisplayName, IsActive, Score)
    VALUES (@Id, @RoundId, @UserId, @DisplayName, @IsActive, @Score);
END

CREATE OR ALTER PROCEDURE [dbo].[usp_CreateMatch]
    @Id UNIQUEIDENTIFIER,
    @RoundId UNIQUEIDENTIFIER,
    @HomeTeam NVARCHAR(255),
    @AwayTeam NVARCHAR(255),
    @StartDateTime DATETIME2,
    @Result TINYINT,
    @IsLocked BIT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO [dbo].[Match] (Id, RoundId, HomeTeam, AwayTeam, StartDateTime, Result, IsLocked)
    VALUES (@Id, @RoundId, @HomeTeam, @AwayTeam, @StartDateTime, @Result, @IsLocked);
END

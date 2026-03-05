CREATE OR ALTER PROCEDURE [dbo].[usp_CreateUser]
    @Id UNIQUEIDENTIFIER,
    @Auth0Id NVARCHAR(255),
    @Email NVARCHAR(255),
    @DisplayName NVARCHAR(255),
    @IsPlatformAdmin BIT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO [dbo].[User] (Id, Auth0Id, Email, DisplayName, IsPlatformAdmin)
    VALUES (@Id, @Auth0Id, @Email, @DisplayName, @IsPlatformAdmin);
END

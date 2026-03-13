CREATE OR ALTER PROCEDURE [dbo].[usp_UpdateUserProfile]
    @Auth0Id NVARCHAR(128),
    @Email NVARCHAR(256),
    @DisplayName NVARCHAR(128)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE [dbo].[User]
    SET Email = @Email,
        DisplayName = @DisplayName
    WHERE Auth0Id = @Auth0Id;
END

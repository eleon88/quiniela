CREATE OR ALTER PROCEDURE [dbo].[usp_GetUserByAuth0Id]
    @Auth0Id NVARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        Id,
        Auth0Id,
        Email,
        DisplayName,
        IsPlatformAdmin
    FROM
        [dbo].[User]
    WHERE
        Auth0Id = @Auth0Id;
END

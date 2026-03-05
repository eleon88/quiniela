CREATE OR ALTER PROCEDURE [dbo].[usp_CreateBoard]
    @Id UNIQUEIDENTIFIER,
    @Name NVARCHAR(255),
    @Description NVARCHAR(MAX),
    @OwnerUserId UNIQUEIDENTIFIER,
    @IsPublic BIT,
    @IsPremium BIT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO [dbo].[Board] (Id, Name, Description, OwnerUserId, IsPublic, IsPremium)
    VALUES (@Id, @Name, @Description, @OwnerUserId, @IsPublic, @IsPremium);
END

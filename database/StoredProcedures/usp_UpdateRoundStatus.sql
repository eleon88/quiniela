CREATE OR ALTER PROCEDURE [dbo].[usp_UpdateRoundStatus]
    @Id UNIQUEIDENTIFIER,
    @Status TINYINT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE [dbo].[Round]
    SET
        Status = @Status
    WHERE
        Id = @Id;
END

CREATE OR ALTER PROCEDURE [dbo].[usp_ActivateParticipant]
    @Id UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE [dbo].[Participant]
    SET
        IsActive = 1
    WHERE
        Id = @Id;
END

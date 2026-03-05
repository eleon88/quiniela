# Stored Procedure Conventions

General structure and best practices when building stored procedures for this project.

## Casing

- SQL keywords should be UPPER CASE

## Aliases

- Table aliases in joins should use the first letter of each word, lowercase
- When duplicates arise, use your judgement (e.g., append a number or use a distinguishing letter)

## Naming

- Format: `[schema].[usp_VerbObjectAdditionalDetails]`
- Examples: `[dbo].[usp_GetBoards]`, `[dbo].[usp_GetBoardById]`, `[dbo].[usp_GetRoundsByBoardId]`

## Structure and Indentation

- See the example below for standard layout

### Example

```sql
CREATE OR ALTER PROCEDURE [dbo].[usp_GetBoardById]
    @BoardId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        b.Id,
        b.Name,
        b.Description,
        b.IsPublic,
        b.IsPremium,
        b.CreatedAt,
        u.DisplayName AS OwnerDisplayName
    FROM
        [dbo].[Board] b
        INNER JOIN [dbo].[User] u ON u.Id = b.OwnerUserId
    WHERE
        b.Id = @BoardId;
END
```

---
name: getting-data-from-db-to-api-dapper
description: A skill for retrieving data from a database and exposing it through a REST API using Dapper
---
# Getting Data from DB to API with Dapper

Create database stored procedures to retrieve data, then implement API endpoints in the .NET 8 backend that call these procedures using Dapper to return JSON responses.

## Key Steps

1. **Design the stored procedure** — follow the naming, casing, and structure conventions in `references/stored-procedure-template.md`
2. **Create the API endpoint** — add a controller action that calls the SP via Dapper and returns the result
3. **Wire up the route** — ensure the endpoint follows the API design in `SPEC.MD` section 6

## Constraints

- **No inline queries** — always use stored procedures for database access
- **Parameterized queries** — use parameters in SPs to prevent SQL injection
- **Input validation** — validate all input parameters before calling the stored procedure
- **Secure endpoints** — apply proper authorization (see `SPEC.MD` sections 6 and 8)

## References

- Stored procedure conventions: `references/stored-procedure-template.md`
- Use the DocsExplorer subagent to look up official Dapper documentation for the latest best practices

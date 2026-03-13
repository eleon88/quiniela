using System.Data;
using Dapper;
using Quiniela.Api.Entities;
using Quiniela.Api.Infrastructure;
using Quiniela.Api.Repositories.Interfaces;

namespace Quiniela.Api.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IDbConnectionFactory _db;

    public UserRepository(IDbConnectionFactory db) => _db = db;

    public async Task<User?> GetByAuth0IdAsync(string auth0Id)
    {
        using var conn = _db.CreateConnection();
        return await conn.QuerySingleOrDefaultAsync<User>(
            "dbo.usp_GetUserByAuth0Id",
            new { Auth0Id = auth0Id },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<User> CreateAsync(User user)
    {
        using var conn = _db.CreateConnection();
        user.Id = Guid.NewGuid();
        await conn.ExecuteAsync(
            "dbo.usp_CreateUser",
            new { user.Id, user.Auth0Id, user.Email, user.DisplayName, user.IsPlatformAdmin },
            commandType: CommandType.StoredProcedure);
        return user;
    }

    public async Task UpdateProfileAsync(string auth0Id, string email, string displayName)
    {
        using var conn = _db.CreateConnection();
        await conn.ExecuteAsync(
            "dbo.usp_UpdateUserProfile",
            new { Auth0Id = auth0Id, Email = email, DisplayName = displayName },
            commandType: CommandType.StoredProcedure);
    }
}

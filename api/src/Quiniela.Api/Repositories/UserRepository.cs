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
            "SELECT * FROM [User] WHERE Auth0Id = @Auth0Id", new { Auth0Id = auth0Id });
    }

    public async Task<User> CreateAsync(User user)
    {
        using var conn = _db.CreateConnection();
        user.Id = Guid.NewGuid();
        await conn.ExecuteAsync(
            @"INSERT INTO [User] (Id, Auth0Id, Email, DisplayName, IsPlatformAdmin)
              VALUES (@Id, @Auth0Id, @Email, @DisplayName, @IsPlatformAdmin)", user);
        return user;
    }
}

using Quiniela.Api.Entities;

namespace Quiniela.Api.Repositories.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByAuth0IdAsync(string auth0Id);
    Task<User> CreateAsync(User user);
    Task UpdateProfileAsync(string auth0Id, string email, string displayName);
}

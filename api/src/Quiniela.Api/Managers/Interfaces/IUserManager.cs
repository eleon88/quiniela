using Quiniela.Api.Entities;

namespace Quiniela.Api.Managers.Interfaces;

public interface IUserManager
{
    Task<User> SyncUserAsync(string auth0Id, string email, string displayName);
    Task<User?> GetByAuth0IdAsync(string auth0Id);
}

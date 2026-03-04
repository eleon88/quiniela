using Quiniela.Api.Entities;

namespace Quiniela.Api.Managers.Interfaces;

public interface IUserManager
{
    Task<User> GetOrCreateByAuth0IdAsync(string auth0Id, string email, string displayName);
}

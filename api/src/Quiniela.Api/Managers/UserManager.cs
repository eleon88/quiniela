using Quiniela.Api.Entities;
using Quiniela.Api.Managers.Interfaces;
using Quiniela.Api.Repositories.Interfaces;

namespace Quiniela.Api.Managers;

public class UserManager : IUserManager
{
    private readonly IUserRepository _userRepo;

    public UserManager(IUserRepository userRepo) => _userRepo = userRepo;

    public async Task<User> GetOrCreateByAuth0IdAsync(string auth0Id, string email, string displayName)
    {
        var user = await _userRepo.GetByAuth0IdAsync(auth0Id);
        if (user != null) return user;

        return await _userRepo.CreateAsync(new User
        {
            Auth0Id = auth0Id,
            Email = email,
            DisplayName = displayName
        });
    }
}

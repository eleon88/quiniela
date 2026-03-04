namespace Quiniela.Api.Entities;

public class User
{
    public Guid Id { get; set; }
    public string Auth0Id { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public bool IsPlatformAdmin { get; set; }
}

using Quiniela.Api.Enums;

namespace Quiniela.Api.Entities;

public class BoardAdmin
{
    public Guid BoardId { get; set; }
    public Guid UserId { get; set; }
    public BoardAdminRole Role { get; set; }
}

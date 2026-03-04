using System.Data;

namespace Quiniela.Api.Infrastructure;

public interface IDbConnectionFactory
{
    IDbConnection CreateConnection();
}

using Microsoft.Data.SqlClient;

namespace SMS.Application.Interfaces.DataAccess
{
    public interface IDbConnectionFactory
    {
        SqlConnection CreateConnection();
    }
}

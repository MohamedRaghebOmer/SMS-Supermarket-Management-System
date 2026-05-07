using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using SMS.Application.Interfaces.DataAccess;

namespace SMS.Infrastructure.Data
{
    public class DbConnectionFactory : IDbConnectionFactory
    {
        private readonly string _connectionString = string.Empty;

        public DbConnectionFactory(IConfiguration configuration)
        {
            _connectionString =
                Environment.GetEnvironmentVariable("SMS_CONNECTION_STRING")
                ?? configuration.GetConnectionString("DefaultConnection")
                ?? throw new Exception("Connection string not found");
        }

        public SqlConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}

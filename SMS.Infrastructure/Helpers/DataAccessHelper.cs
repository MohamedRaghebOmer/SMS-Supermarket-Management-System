using Microsoft.Data.SqlClient;
using SMS.Infrastructure.Enums;
using SMS.Application.Common.Results;
using SMS.Application.Interfaces.DataAccess;
using System.Data;

namespace SMS.Infrastructure.Helpers
{
    internal class DataAccessHelper : IDbHelper
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public DataAccessHelper(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }


        public SqlConnection CreateConnection()
        {
            return _connectionFactory.CreateConnection();
        }

        public SqlCommand CreateCommand(SqlConnection conn, string spName)
        {
            return new SqlCommand(spName, conn)
            {
                CommandType = CommandType.StoredProcedure
            };
        }

        public void AddDefaultParameters(SqlCommand cmd, out SqlParameter code, out SqlParameter message)
        {
            // Output parameters
            code = new SqlParameter("@StatusCode", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };

            message = new SqlParameter("@StatusMessage", SqlDbType.NVarChar, 4000)
            {
                Direction = ParameterDirection.Output
            };

            // Add output parameters to the command
            cmd.Parameters.Add(code);
            cmd.Parameters.Add(message);
        }

        public OperationResult<T> CreateOperationResult<T>(T data, SqlParameter code, SqlParameter message)
        {
            return new OperationResult<T>(
                data,
                (OperationStatus)(int)code.Value,
                message?.Value.ToString() ?? string.Empty);
        }

        public OperationResult<bool> CreateOperationResult(SqlParameter code, SqlParameter message)
        {
            OperationStatus status = (OperationStatus)(int)code.Value;

            return new OperationResult<bool>(
                status == OperationStatus.Success,
                status,
                message?.Value.ToString() ?? string.Empty);
        }
    }
}

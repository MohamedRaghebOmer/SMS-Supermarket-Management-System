using Microsoft.Data.SqlClient;
using SMS.Application.Common.Enums;
using SMS.Application.Common.Results;
using SMS.Application.Interfaces.DataAccess;
using SMS.Shared.Common;
using SMS.Shared.Constants;
using System.Data;

namespace SMS.Infrastructure.Helpers
{
    internal class DataAccessHelper : IDataAccessHelper
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

        public OperationResult<T?> CreateOperationResult<T>(T? data, SqlParameter code, SqlParameter message)
        {
            return new OperationResult<T?>(
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

        public async Task<PaginationResponse<T>> ReadPaginationAsync<T>(SqlCommand cmd, PaginationRequest paginationRequest, Func<SqlDataReader, T> mapFunc)
        {
            var items = new List<T>();

            int totalCount = -1;
            using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    items.Add(mapFunc(reader));

                    if (totalCount == -1)
                    {
                        // After this line, the total count value will change to the actual count from the database,
                        // and we won't read it again, and it's impossible to be -1 in the database,
                        // so we can use this value to determine whether we have read the total count or not.
                        totalCount = reader.GetInt32(reader.GetOrdinal(Constants.PaginationResponseTotalCountParamName));
                    }
                }
            }

            return new PaginationResponse<T>
            {
                Items = items,
                TotalCount = totalCount,
                Page = paginationRequest.Page,
                PageSize = paginationRequest.PageSize
            };
        }
    }
}

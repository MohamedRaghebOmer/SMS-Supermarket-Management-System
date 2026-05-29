using Microsoft.Data.SqlClient;
using SMS.Application.Common.Enums;
using SMS.Application.Common.Results;
using SMS.Application.Interfaces.DataAccess;
using SMS.Shared.Common;
using System.Data;

namespace SMS.Infrastructure.Helpers
{
    internal sealed class StoredProcedureExecutor : IStoredProcedureExecutor
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public StoredProcedureExecutor(IDbConnectionFactory connectionFactory)
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

        public void AttachStatusParameters(SqlCommand cmd, out SqlParameter code, out SqlParameter message)
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

        public async Task<(SqlParameter code, SqlParameter message)> PrepareCommandAsync(SqlCommand cmd, SqlConnection conn)
        {
            AttachStatusParameters(cmd, out SqlParameter code, out SqlParameter message);
            await conn.OpenAsync();
            return (code, message);
        }

        public OperationResult<T?> CreateOperationResult<T>(T? data, SqlParameter code, SqlParameter message)
        {
            return new OperationResult<T?>(
                data,
                (OperationStatus)(int)code.Value,
                message.Value?.ToString() ?? string.Empty);
        }

        public OperationResult<bool> CreateOperationResult(SqlParameter code, SqlParameter message)
        {
            OperationStatus status = (OperationStatus)(int)code.Value;

            return new OperationResult<bool>(
                status == OperationStatus.Success,
                status,
                message.Value?.ToString() ?? string.Empty);
        }

        public async Task<PaginationResponse<T>> ReadPaginationAsync<T>(SqlCommand cmd, PaginationRequest paginationRequest, Func<SqlDataReader, T> mapFunc)
        {
            var items = new List<T>();

            int totalCount = 0;
            using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    items.Add(mapFunc(reader));

                    if (totalCount == 0)
                    {
                        // After this line, the total count value will change to the actual count from the database,
                        // and we won't read it again, and it's impossible to be -1 in the database,
                        // so we can use this value to determine whether we have read the total count or not.
                        totalCount = reader.GetInt32(reader.GetOrdinal("TotalCount"));
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

        public async Task<OperationResult<PaginationResponse<T>>> ExecutePaginationAsync<T>(
            SqlCommand cmd, SqlConnection conn, PaginationRequest paginationRequest,
            Func<SqlDataReader, T> mapFunc)
        {
            cmd.Parameters.Add("@Page", SqlDbType.Int).Value = paginationRequest.Page;
            cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = paginationRequest.PageSize;
            var (statusCodeOutParam, statusMessageOutParam) = await PrepareCommandAsync(cmd, conn);

            var pagination = await ReadPaginationAsync(cmd, paginationRequest, mapFunc);

            return CreateOperationResult(pagination, statusCodeOutParam, statusMessageOutParam);
        }

        public async Task<OperationResult<T?>> ExecuteSingleAsync<T>(SqlCommand cmd, SqlConnection conn, Func<SqlDataReader, T> mapFunc)
        {
            var (statusCodeOutParam, statusMessageOutParam) = await PrepareCommandAsync(cmd, conn);

            T? result = default;
            using (SqlDataReader reader = await cmd.ExecuteReaderAsync(CommandBehavior.SingleRow))
            {
                if (await reader.ReadAsync())
                {
                    result = mapFunc(reader);
                }
            }

            return CreateOperationResult(result, statusCodeOutParam, statusMessageOutParam);
        }

        public async Task<OperationResult<IReadOnlyList<T>>> ExecuteListAsync<T>(SqlCommand cmd, SqlConnection conn, Func<SqlDataReader, T> mapFunc)
        {
            var (statusCodeOutParam, statusMessageOutParam) = await PrepareCommandAsync(cmd, conn);

            var results = new List<T>();
            using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    results.Add(mapFunc(reader));
                }
            }

            return CreateOperationResult((IReadOnlyList<T>)results, statusCodeOutParam, statusMessageOutParam);
        }

        public async Task<OperationResult<bool>> ExecuteNonQueryAsync(SqlCommand cmd, SqlConnection conn)
        {
            var (statusCodeOutParam, statusMessageOutParam) = await PrepareCommandAsync(cmd, conn);

            await cmd.ExecuteNonQueryAsync();

            return CreateOperationResult(statusCodeOutParam, statusMessageOutParam);
        }

        public async Task<OperationResult<T?>> ExecuteNonQueryAsync<T>(SqlCommand cmd, SqlConnection conn, SqlParameter operationResultDataParam)
        {
            var (statusCodeOutParam, statusMessageOutParam) = await PrepareCommandAsync(cmd, conn);

            await cmd.ExecuteNonQueryAsync();

            return CreateOperationResult((T?)operationResultDataParam.Value, statusCodeOutParam, statusMessageOutParam);
        }

        public async Task<OperationResult<T>> ExecuteScalarAsync<T>(SqlCommand cmd,
            SqlConnection conn) where T : IConvertible
        {
            var (statusCodeOutParam, statusMessageOutParam) = await PrepareCommandAsync(cmd, conn);

            object? scalarResult = await cmd.ExecuteScalarAsync();

            T result = default!;
            if (scalarResult != null && scalarResult != DBNull.Value)
            {
                result = (T)Convert.ChangeType(scalarResult, typeof(T));
            }

            return CreateOperationResult(result, statusCodeOutParam, statusMessageOutParam);
        }

        public async Task<OperationResult<bool>> ExecuteExistsAsync(SqlConnection conn, SqlCommand cmd)
        {
            var (statusCodeOutParam, statusMessageOutParam) = await PrepareCommandAsync(cmd, conn);

            var result = await cmd.ExecuteScalarAsync() != null;

            return CreateOperationResult(result, statusCodeOutParam, statusMessageOutParam);
        }

        public async Task<OperationResult<bool>> ExecuteBooleanOperationAsync(
            SqlCommand cmd, SqlConnection conn)
        {
            AttachStatusParameters(cmd, out SqlParameter code, out SqlParameter message);

            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();

            OperationStatus codeValue = code.Value != null ? (OperationStatus)(int)code.Value : OperationStatus.UnexpectedError;

            return CreateOperationResult<bool>(codeValue == OperationStatus.Success, code, message);
        }
    }
}
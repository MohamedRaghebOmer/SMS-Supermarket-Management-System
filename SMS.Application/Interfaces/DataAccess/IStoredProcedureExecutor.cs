using Microsoft.Data.SqlClient;
using SMS.Application.Common.Results;
using SMS.Shared.Common;

namespace SMS.Application.Interfaces.DataAccess
{
    public interface IStoredProcedureExecutor
    {
        SqlConnection CreateConnection();
        SqlCommand CreateCommand(SqlConnection conn, string spName);
        void AttachStatusParameters(SqlCommand cmd, out SqlParameter code, out SqlParameter message);
        Task<(SqlParameter code, SqlParameter message)> PrepareCommandAsync(SqlCommand cmd, SqlConnection conn);
        OperationResult<T?> CreateOperationResult<T>(T? data, SqlParameter codeParam, SqlParameter messageParam);
        OperationResult<bool> CreateOperationResult(SqlParameter codeParam, SqlParameter messageParam);
        Task<PaginationResponse<T>> ReadPaginationAsync<T>(SqlCommand cmd, PaginationRequest paginationRequest, Func<SqlDataReader, T> mapFunc);
        Task<OperationResult<PaginationResponse<T>>> ExecutePaginationAsync<T>(
            SqlCommand cmd, SqlConnection conn, PaginationRequest paginationRequest,
            Func<SqlDataReader, T> mapFunc);
        Task<OperationResult<T?>> ExecuteSingleAsync<T>(SqlCommand cmd, SqlConnection conn, Func<SqlDataReader, T> mapFunc);
        Task<OperationResult<IReadOnlyList<T>>> ExecuteListAsync<T>(SqlCommand cmd, SqlConnection conn, Func<SqlDataReader, T> mapFunc);
        Task<OperationResult<bool>> ExecuteNonQueryAsync(SqlCommand cmd, SqlConnection conn);
        Task<OperationResult<T>> ExecuteScalarAsync<T>(SqlCommand cmd, SqlConnection conn) where T : IConvertible;
        Task<OperationResult<T?>> ExecuteNonQueryAsync<T>(SqlCommand cmd, SqlConnection conn, SqlParameter operationResultDataParam);
        Task<OperationResult<decimal>> ExecuteDecimalScalarAsync(string storedProcedure);
        Task<OperationResult<int>> ExecuteIntScalarAsync(string storedProcedure);
        Task<OperationResult<bool>> ExecuteBoolScalarAsync(string storedProcedure);
        Task<OperationResult<bool>> ExecuteDecimalUpdateAsync(string storedProcedure, string parameterName, decimal value);
        Task<OperationResult<bool>> ExecuteIntUpdateAsync(string storedProcedure, string parameterName, int value);
        Task<OperationResult<bool>> ExecuteBoolUpdateAsync(string storedProcedure, string parameterName, bool value);
        Task<OperationResult<bool>> ExecuteExistsAsync(SqlConnection conn, SqlCommand cmd);
        Task<OperationResult<bool>> ExecuteBooleanOperationAsync(SqlCommand cmd, SqlConnection conn);
    }
}

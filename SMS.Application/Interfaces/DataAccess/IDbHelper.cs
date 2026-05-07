using Microsoft.Data.SqlClient;
using SMS.Application.Common.Results;

namespace SMS.Application.Interfaces.DataAccess
{
    public interface IDbHelper
    {
        SqlConnection CreateConnection();
        SqlCommand CreateCommand(SqlConnection conn, string spName);
        void AddDefaultParameters(SqlCommand cmd, out SqlParameter code, out SqlParameter message);
        OperationResult<T> CreateOperationResult<T>(T data, SqlParameter codeParam, SqlParameter messageParam);
        OperationResult<bool> CreateOperationResult(SqlParameter codeParam, SqlParameter messageParam);
    }
}

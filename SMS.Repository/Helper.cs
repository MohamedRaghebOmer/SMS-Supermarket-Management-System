using SMS.Core;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace SMS.Repository
{
    internal static class Helper
    {
        public static readonly string ConnectionString = ConfigurationManager.ConnectionStrings["SMSConnectionString"].ConnectionString;

        public static SqlConnection CreateConnection()
        {
            return new SqlConnection(Helper.ConnectionString);
        }

        public static SqlCommand CreateCommand(SqlConnection conn, string spName)
        {
            return new SqlCommand(spName, conn)
            {
                CommandType = CommandType.StoredProcedure
            };
        }

        public static void AddStatusParams(SqlCommand cmd, out SqlParameter code, out SqlParameter message)
        {
            code = new SqlParameter("@StatusCode", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };

            message = new SqlParameter("@StatusMessage", SqlDbType.NVarChar, 4000)
            {
                Direction = ParameterDirection.Output
            };

            cmd.Parameters.Add(code);
            cmd.Parameters.Add(message);
        }

        public static async Task<DataTable> ExecuteDataTableAsync(SqlCommand cmd)
        {
            DataTable dt = new DataTable();

            using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
            {
                if (reader.HasRows)
                {
                    dt.Load(reader);
                }
            }

            return dt;
        }

        public static DBResponse<T> CreateDBResponse<T>(SqlParameter data, SqlParameter code, SqlParameter message)
        {
            return new DBResponse<T>()
            {
                Data = (T)data.Value,
                Code = (StatusCode)(int)code.Value,
                Message = message.Value as string
            };
        }

        public static DBResponse<T> CreateDBResponse<T>(T data, SqlParameter code, SqlParameter message)
        {
            return new DBResponse<T>()
            {
                Data = data,
                Code = (StatusCode)(int)code.Value,
                Message = message.Value as string
            };
        }

        public static DBResponse<bool> CreateDBResponse(SqlParameter code, SqlParameter message)
        {
            return new DBResponse<bool>()
            {
                Data = (StatusCode)(int)code.Value == StatusCode.Success,
                Code = (StatusCode)(int)code.Value,
                Message = message.Value as string
            };
        }
    }
}
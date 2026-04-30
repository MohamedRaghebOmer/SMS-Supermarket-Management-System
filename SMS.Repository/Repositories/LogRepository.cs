using SMS.Core;
using SMS.Core.DTOs.Enums;
using SMS.Core.Interfaces;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace SMS.Repository
{
    public class LogRepository : IDatabaseLogger
    {
        public async Task<DBResponse<int>> LogAsync(LogLevel level, string message, Exception ex, string source)
        {
            DBResponse<int> response = new DBResponse<int>();

            try
            {
                using (var conn = Helper.CreateConnection())
                using (var cmd = Helper.CreateCommand(conn, "usp_Logs_Insert"))
                {
                    cmd.Parameters.Add("@LogLevel", SqlDbType.TinyInt).Value = (byte)level;
                    cmd.Parameters.Add("@Message", SqlDbType.NVarChar, -1).Value = message;
                    cmd.Parameters.Add("@Exception", SqlDbType.NVarChar, -1).Value = ex?.ToString() ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@StackTrace", SqlDbType.NVarChar, -1).Value = ex?.StackTrace ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@Source", SqlDbType.NVarChar, 100).Value = source ?? (object)DBNull.Value;
                    Helper.AddDefaultParameters(cmd, out SqlParameter codeParam, out SqlParameter messageParam);

                    SqlParameter insertedId = new SqlParameter("@InsertedLogId", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(insertedId);

                    await conn.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();

                    response = Helper.CreateDBResponse<int>(insertedId, codeParam, messageParam);
                }
            }
            catch
            {
                // ignore logging failures
                // Never throw from a logger, otherwise you risk crashing the app when logging fails
            }

            return response;
        }
    }
}

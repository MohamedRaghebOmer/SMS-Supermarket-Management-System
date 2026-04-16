using SMS.Core;
using SMS.Core.Logging;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace SMS.Repository
{
    public static class LogsRepository
    {
        public static async Task<DBResponse<bool>> LogAsync(Logger.LoggerEventArgs loggerEventArgs)
        {
            DBResponse<bool> response = new DBResponse<bool>();

            try
            {
                using (var conn = Helper.CreateConnection())
                using (var cmd = Helper.CreateCommand(conn, "spLogs_Insert"))
                {
                    cmd.Parameters.Add("@LogLevel", SqlDbType.TinyInt).Value = (byte)loggerEventArgs.Level;
                    cmd.Parameters.Add("@Message", SqlDbType.NVarChar, -1).Value = loggerEventArgs.Message;
                    cmd.Parameters.Add("@Exception", SqlDbType.NVarChar, -1).Value = loggerEventArgs.Exception?.ToString() ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@Source", SqlDbType.NVarChar, 100).Value = loggerEventArgs.Source ?? (object)DBNull.Value;
                    Helper.AddStatusParams(cmd, out SqlParameter codeParam, out SqlParameter messageParam);

                    await conn.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();

                    response = Helper.CreateDBResponse(codeParam, messageParam);
                }
            }
            catch
            {
                // ignore logging failures
            }

            return response;
        }
    }
}

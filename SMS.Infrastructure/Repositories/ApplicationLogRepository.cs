using Microsoft.Data.SqlClient;
using SMS.Application.Common.Results;
using SMS.Application.Interfaces.DataAccess;
using SMS.Application.Interfaces.Repositories;
using SMS.Contracts.Responses;
using SMS.Domain.Entities;
using System.Data;
using LogLevel = SMS.Shared.Enums.LogLevel;

namespace SMS.Infrastructure.Repositories
{
    public class ApplicationLogRepository : IApplicationLogRepository
    {
        private readonly IDataAccessHelper _helper;

        public ApplicationLogRepository(IDataAccessHelper helper)
        {
            _helper = helper;
        }

        public async Task<OperationResult<int>> AddAsync(ApplicationLog log)
        {
            using (var conn = _helper.CreateConnection())
            using (var cmd = _helper.CreateCommand(conn, "usp_ApplicationLogs_Insert"))
            {
                cmd.Parameters.Add("@AuditLogId", SqlDbType.Int).Value = log.AuditLogId ?? (object)DBNull.Value;
                cmd.Parameters.Add("@Message", SqlDbType.NVarChar, -1).Value = log.Message;
                cmd.Parameters.Add("@Exception", SqlDbType.NVarChar, -1).Value = log.Exception?.ToString() ?? (object)DBNull.Value;
                cmd.Parameters.Add("@StackTrace", SqlDbType.NVarChar, -1).Value = log.StackTrace ?? (object)DBNull.Value;

                var insertedIdParam = new SqlParameter("@InsertedId", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };

                cmd.Parameters.Add(insertedIdParam);

                _helper.AddDefaultParameters(cmd, out SqlParameter code, out SqlParameter message);

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();

                return _helper.CreateOperationResult((int)insertedIdParam.Value, code, message);
            }
        }

        public async Task<OperationResult<ApplicationLog?>> FindAsync(int id)
        {
            using (var conn = _helper.CreateConnection())
            using (var cmd = _helper.CreateCommand(conn, "usp_ApplicationLogs_GetById"))
            {
                cmd.Parameters.Add("@ApplicationLogId", SqlDbType.Int).Value = id;
                _helper.AddDefaultParameters(cmd, out SqlParameter code, out SqlParameter message);

                await conn.OpenAsync();
                using var reader = await cmd.ExecuteReaderAsync(CommandBehavior.SingleRow);

                var log = await MapApplicationLogAsync(reader);

                return _helper.CreateOperationResult(log, code, message);
            }
        }

        public async Task<OperationResult<IReadOnlyList<ApplicationLogResponseDto>>> FindByAuditLogIdAsync(int auditLogId)
        {
            using (var conn = _helper.CreateConnection())
            using (var cmd = _helper.CreateCommand(conn, "usp_ApplicationLogs_GetByAuditLogId"))
            {
                cmd.Parameters.Add("@AuditLogId", SqlDbType.Int).Value = auditLogId;

                _helper.AddDefaultParameters(cmd, out SqlParameter code, out SqlParameter message);

                await conn.OpenAsync();
                var logs = await ReadApplicationLogResponsesAsync(cmd);

                return _helper.CreateOperationResult<IReadOnlyList<ApplicationLogResponseDto>>(logs, code, message);
            }
        }

        public async Task<OperationResult<IReadOnlyList<ApplicationLog>>> GetPagedAsync(int page, int pageSize)
        {
            using (var conn = _helper.CreateConnection())
            using (var cmd = _helper.CreateCommand(conn, "usp_ApplicationLogs_GetPaged"))
            {
                cmd.Parameters.Add("@Page", SqlDbType.Int).Value = page;
                cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = pageSize;

                _helper.AddDefaultParameters(cmd, out SqlParameter code, out SqlParameter message);

                await conn.OpenAsync();
                var logs = await ReadApplicationLogsAsync(cmd);

                return _helper.CreateOperationResult<IReadOnlyList<ApplicationLog>>(logs, code, message);
            }
        }

        public async Task<OperationResult<IReadOnlyList<ApplicationLog>>> GetPagedByLogLevelAsync(LogLevel logLevel, int page, int pageSize)
        {
            using (var conn = _helper.CreateConnection())
            using (var cmd = _helper.CreateCommand(conn, "usp_ApplicationLogs_GetPagedByLogLevel"))
            {
                cmd.Parameters.Add("@LogLevel", SqlDbType.Int).Value = (int)logLevel;
                cmd.Parameters.Add("@Page", SqlDbType.Int).Value = page;
                cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = pageSize;

                _helper.AddDefaultParameters(cmd, out SqlParameter code, out SqlParameter message);

                await conn.OpenAsync();
                var logs = await ReadApplicationLogsAsync(cmd);

                return _helper.CreateOperationResult<IReadOnlyList<ApplicationLog>>(logs, code, message);
            }
        }

        public async Task<OperationResult<IReadOnlyList<ApplicationLog>>> GetPagedByDateRangeAsync(DateTime startDate, DateTime endDate, int page, int pageSize)
        {
            using (var conn = _helper.CreateConnection())
            using (var cmd = _helper.CreateCommand(conn, "usp_ApplicationLogs_GetPagedByDateRange"))
            {
                cmd.Parameters.Add("@StartDate", SqlDbType.DateTime2).Value = startDate;
                cmd.Parameters.Add("@EndDate", SqlDbType.DateTime2).Value = endDate;
                cmd.Parameters.Add("@Page", SqlDbType.Int).Value = page;
                cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = pageSize;

                _helper.AddDefaultParameters(cmd, out SqlParameter code, out SqlParameter message);

                await conn.OpenAsync();
                var logs = await ReadApplicationLogsAsync(cmd);

                return _helper.CreateOperationResult<IReadOnlyList<ApplicationLog>>(logs, code, message);
            }
        }



        private static async Task<ApplicationLog?> MapApplicationLogAsync(SqlDataReader reader)
        {
            if (!reader.HasRows)
            {
                return null;
            }

            if (!await reader.ReadAsync())
            {
                return null;
            }

            return MapApplicationLog(reader);
        }

        private static ApplicationLog MapApplicationLog(SqlDataReader reader)
        {
            var auditLogIdOrdinal = reader.GetOrdinal("AuditLogId");
            int? auditLogId = reader.IsDBNull(auditLogIdOrdinal) ? null : reader.GetInt32(auditLogIdOrdinal);

            var exceptionOrdinal = reader.GetOrdinal("Exception");
            string? exceptionMessage = reader.IsDBNull(exceptionOrdinal) ? null : reader.GetString(exceptionOrdinal);
            Exception? exception = exceptionMessage is null ? null : new Exception(exceptionMessage);

            var stackTraceOrdinal = reader.GetOrdinal("StackTrace");
            string? stackTrace = reader.IsDBNull(stackTraceOrdinal) ? null : reader.GetString(stackTraceOrdinal);

            return new ApplicationLog(
                applicationLogId: reader.GetInt32(reader.GetOrdinal("ApplicationLogId")),
                auditLogId: auditLogId,
                message: reader.GetString(reader.GetOrdinal("Message")),
                exception: exception,
                stackTrace: stackTrace);
        }

        private static async Task<IReadOnlyList<ApplicationLog>> ReadApplicationLogsAsync(SqlCommand cmd)
        {
            var logs = new List<ApplicationLog>();

            using (var reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    logs.Add(MapApplicationLog(reader));
                }
            }

            return logs;
        }

        private static async Task<IReadOnlyList<ApplicationLogResponseDto>> ReadApplicationLogResponsesAsync(SqlCommand cmd)
        {
            var logs = new List<ApplicationLogResponseDto>();

            using (var reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    logs.Add(MapApplicationLogResponse(reader));
                }
            }

            return logs;
        }

        private static ApplicationLogResponseDto MapApplicationLogResponse(SqlDataReader reader)
        {
            var auditLogIdOrdinal = reader.GetOrdinal("AuditLogId");
            int? auditLogId = reader.IsDBNull(auditLogIdOrdinal) ? null : reader.GetInt32(auditLogIdOrdinal);

            var exceptionOrdinal = reader.GetOrdinal("Exception");
            Exception? exception = reader.IsDBNull(exceptionOrdinal) ? null : new Exception(reader.GetString(exceptionOrdinal));

            var stackTraceOrdinal = reader.GetOrdinal("StackTrace");
            string? stackTrace = reader.IsDBNull(stackTraceOrdinal) ? null : reader.GetString(stackTraceOrdinal);

            return new ApplicationLogResponseDto
            {
                ApplicationLogId = reader.GetInt32(reader.GetOrdinal("ApplicationLogId")),
                AuditLogId = auditLogId,
                Message = reader.GetString(reader.GetOrdinal("Message")),
                Exception = exception,
                StackTrace = stackTrace
            };
        }
    }
}

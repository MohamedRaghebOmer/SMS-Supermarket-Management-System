using Microsoft.Data.SqlClient;
using SMS.Application.Common.Results;
using SMS.Application.Interfaces.DataAccess;
using SMS.Application.Interfaces.Repositories;
using SMS.Domain.Entities;
using SMS.Shared.Common;
using System.Data;
using LogLevel = SMS.Shared.Enums.LogLevel;

namespace SMS.Infrastructure.Repositories
{
    public class ApplicationLogRepository : IApplicationLogRepository
    {
        private readonly IStoredProcedureExecutor _executor;

        public ApplicationLogRepository(IStoredProcedureExecutor helper)
        {
            _executor = helper;
        }


        public async Task<OperationResult<int>> AddAsync(ApplicationLog log)
        {
            await using var conn = _executor.CreateConnection();
            using var cmd = _executor.CreateCommand(conn, "usp_ApplicationLogs_Insert");

            AddApplicationLogParameters(cmd, log);
            var insertedIdParam = new SqlParameter("@InsertedId", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(insertedIdParam);

            return await _executor.ExecuteNonQueryAsync<int>(cmd, conn, insertedIdParam);
        }

        public async Task<OperationResult<ApplicationLog?>> FindAsync(int id)
        {
            await using var conn = _executor.CreateConnection();
            using var cmd = _executor.CreateCommand(conn, "usp_ApplicationLogs_GetById");

            cmd.Parameters.Add("@ApplicationLogId", SqlDbType.Int).Value = id;
            return await _executor.ExecuteSingleAsync(cmd, conn, MapApplicationLog);
        }

        public async Task<OperationResult<ApplicationLog?>> FindByAuditLogIdAsync(long auditLogId)
        {
            await using var conn = _executor.CreateConnection();
            using var cmd = _executor.CreateCommand(conn, "usp_ApplicationLogs_GetByAuditLogId");

            cmd.Parameters.Add("@AuditLogId", SqlDbType.BigInt).Value = auditLogId;

            return await _executor.ExecuteSingleAsync(cmd, conn, MapApplicationLog);
        }

        public async Task<OperationResult<PaginationResponse<ApplicationLog>>> GetPagedAsync(PaginationRequest paginationRequest)
        {
            await using var conn = _executor.CreateConnection();
            using var cmd = _executor.CreateCommand(conn, "usp_ApplicationLogs_GetPaged");

            return await _executor.ExecutePaginationAsync(cmd, conn, paginationRequest, MapApplicationLog);
        }

        public async Task<OperationResult<PaginationResponse<ApplicationLog>>>
            GetPagedByLogLevelAsync(LogLevel logLevel, PaginationRequest paginationRequest)
        {
            await using var conn = _executor.CreateConnection();
            using var cmd = _executor.CreateCommand(conn, "usp_ApplicationLogs_GetPagedByLogLevel");

            cmd.Parameters.Add("@LogLevel", SqlDbType.TinyInt).Value = (byte)logLevel;
            return await _executor.ExecutePaginationAsync(cmd, conn, paginationRequest, MapApplicationLog);
        }

        public async Task<OperationResult<PaginationResponse<ApplicationLog>>>
            GetPagedByDateRangeAsync(DateTime startDate, DateTime endDate, PaginationRequest paginationRequest)
        {
            await using var conn = _executor.CreateConnection();
            using var cmd = _executor.CreateCommand(conn, "usp_ApplicationLogs_GetPagedByDateRange");

            cmd.Parameters.Add("@StartDate", SqlDbType.DateTime2).Value = startDate;
            cmd.Parameters.Add("@EndDate", SqlDbType.DateTime2).Value = endDate;
            return await _executor.ExecutePaginationAsync(cmd, conn, paginationRequest, MapApplicationLog);
        }



        private static ApplicationLog MapApplicationLog(SqlDataReader reader)
        {
            var auditLogIdOrdinal = reader.GetOrdinal("AuditLogId");
            long? auditLogId = reader.IsDBNull(auditLogIdOrdinal) ? null : reader.GetInt64(auditLogIdOrdinal);

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

        private static void AddApplicationLogParameters(SqlCommand cmd, ApplicationLog log, bool includeId = false)
        {
            if (includeId)
            {
                cmd.Parameters.Add("@ApplicationLogId", SqlDbType.Int).Value = log.ApplicationLogId;
            }

            cmd.Parameters.Add("@AuditLogId", SqlDbType.BigInt).Value = log.AuditLogId ?? (object)DBNull.Value;
            cmd.Parameters.Add("@LogLevel", SqlDbType.TinyInt).Value = (byte)log.LogLevel;
            cmd.Parameters.Add("@Message", SqlDbType.NVarChar, -1).Value = log.Message;
            cmd.Parameters.Add("@Exception", SqlDbType.NVarChar, -1).Value = log.Exception?.ToString() ?? (object)DBNull.Value;
            cmd.Parameters.Add("@StackTrace", SqlDbType.NVarChar, -1).Value = log.StackTrace ?? (object)DBNull.Value;
        }
    }
}
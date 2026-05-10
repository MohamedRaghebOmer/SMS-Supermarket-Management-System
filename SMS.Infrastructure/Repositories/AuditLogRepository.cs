using Microsoft.Data.SqlClient;
using SMS.Application.Common.Results;
using SMS.Application.Interfaces.DataAccess;
using SMS.Application.Interfaces.Repositories;
using SMS.Domain.Entities;
using SMS.Shared.Enums;
using System.Data;
using System.Net;

namespace SMS.Infrastructure.Repositories
{
    public class AuditLogRepository : IAuditLogRepository
    {
        private readonly IDataAccessHelper _helper;

        public AuditLogRepository(IDataAccessHelper helper)
        {
            _helper = helper;
        }

        public async Task<OperationResult<int>> AddAuditLogAsync(AuditLog auditLog)
        {
            using (var conn = _helper.CreateConnection())
            using (var cmd = _helper.CreateCommand(conn, "usp_AuditLogs_Insert"))
            {
                cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = auditLog.UserId ?? (object)DBNull.Value;
                cmd.Parameters.Add("@AttemptedLoginIdentifier", SqlDbType.NVarChar, 100).Value =
                    auditLog.AttemptedLoginIdentifier ?? (object)DBNull.Value;
                cmd.Parameters.Add("@CorrelationId", SqlDbType.UniqueIdentifier).Value = auditLog.RequestGuid;
                cmd.Parameters.Add("@ActionType", SqlDbType.TinyInt).Value = (byte)auditLog.ActionType;
                cmd.Parameters.Add("@Endpoint", SqlDbType.NVarChar, 200).Value = auditLog.Endpoint;
                cmd.Parameters.Add("@RequestBody", SqlDbType.NVarChar, -1).Value = auditLog.RequestBody ?? (object)DBNull.Value;
                cmd.Parameters.Add("@ResponseBody", SqlDbType.NVarChar, -1).Value = auditLog.ResponseBody ?? (object)DBNull.Value;
                cmd.Parameters.Add("@UserAgent", SqlDbType.NVarChar, 300).Value = auditLog.UserAgent ?? (object)DBNull.Value;
                cmd.Parameters.Add("@StatusCode", SqlDbType.Int).Value = (int)auditLog.StatusCode;
                cmd.Parameters.Add("@IsSuccess", SqlDbType.Bit).Value = auditLog.IsSuccess;
                cmd.Parameters.Add("@Duration", SqlDbType.Int).Value = auditLog.Duration;
                cmd.Parameters.Add("@IpAddress", SqlDbType.NVarChar, 50).Value = auditLog.IpAddress;

                var detailsParam = cmd.Parameters.Add("@Details", SqlDbType.NVarChar, -1);
                detailsParam.Value = auditLog.Details is null ? DBNull.Value : auditLog.Details;

                SqlParameter insertedIdParam = new SqlParameter("@InsertedId", SqlDbType.Int)
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

        public async Task<OperationResult<AuditLog?>> FindAsync(int auditLogId)
        {
            using (SqlConnection conn = _helper.CreateConnection())
            using (SqlCommand cmd = _helper.CreateCommand(conn, "usp_AuditLogs_GetById"))
            {
                cmd.Parameters.Add("@AuditLogId", SqlDbType.BigInt).Value = auditLogId;
                _helper.AddDefaultParameters(cmd, out SqlParameter code, out SqlParameter message);

                await conn.OpenAsync();
                AuditLog? auditLog = MapAuditLog(await cmd.ExecuteReaderAsync(CommandBehavior.SingleRow));

                return _helper.CreateOperationResult(auditLog, code, message);
            }
        }

        public async Task<OperationResult<AuditLog?>> FindByCorrelationIdAsync(Guid correlationId)
        {
            using (SqlConnection conn = _helper.CreateConnection())
            using (SqlCommand cmd = _helper.CreateCommand(conn, "usp_AuditLogs_GetByCorrelationId"))
            {
                cmd.Parameters.Add("@CorrelationId", SqlDbType.UniqueIdentifier).Value = correlationId;
                _helper.AddDefaultParameters(cmd, out SqlParameter code, out SqlParameter message);

                await conn.OpenAsync();
                AuditLog? auditLog = MapAuditLog(await cmd.ExecuteReaderAsync(CommandBehavior.SingleRow));

                return _helper.CreateOperationResult(auditLog, code, message);
            }
        }

        public async Task<OperationResult<IReadOnlyList<AuditLog>>> GetPagedAsync(int pageNumber, int pageSize)
        {
            using (var conn = _helper.CreateConnection())
            using (var cmd = _helper.CreateCommand(conn, "usp_AuditLogs_GetPaged"))
            {
                cmd.Parameters.Add("@PageNumber", SqlDbType.Int).Value = pageNumber;
                cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = pageSize;
                _helper.AddDefaultParameters(cmd, out SqlParameter statusCodeOutParam, out SqlParameter statusMessageOutParam);

                await conn.OpenAsync();
                var auditLogs = await ReadAuditLogsAsync(cmd);

                return _helper.CreateOperationResult(auditLogs, statusCodeOutParam, statusMessageOutParam);
            }
        }

        public async Task<OperationResult<IReadOnlyList<AuditLog>>> GetByUserIdPagedAsync(int userId, int pageNumber, int pageSize)
        {
            using (var conn = _helper.CreateConnection())
            using (var cmd = _helper.CreateCommand(conn, "usp_AuditLogs_GetByUserIdPaged"))
            {
                cmd.Parameters.Add("@UserId", SqlDbType.TinyInt).Value = userId;
                cmd.Parameters.Add("@PageNumber", SqlDbType.Int).Value = pageNumber;
                cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = pageSize;
                _helper.AddDefaultParameters(cmd, out SqlParameter statusCodeOutParam, out SqlParameter statusMessageOutParam);

                await conn.OpenAsync();
                var auditLogs = await ReadAuditLogsAsync(cmd);

                return _helper.CreateOperationResult<IReadOnlyList<AuditLog>>(auditLogs, statusCodeOutParam, statusMessageOutParam);
            }
        }

        public async Task<OperationResult<IReadOnlyList<AuditLog>>> GetByActionTypePagedAsync(AuditActionType actionType, int pageNumber, int pageSize)
        {
            using (var conn = _helper.CreateConnection())
            using (var cmd = _helper.CreateCommand(conn, "usp_AuditLogs_GetByActionTypePaged"))
            {
                cmd.Parameters.Add("@ActionType", SqlDbType.TinyInt).Value = (int)actionType;
                cmd.Parameters.Add("@PageNumber", SqlDbType.Int).Value = pageNumber;
                cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = pageSize;
                _helper.AddDefaultParameters(cmd, out SqlParameter statusCodeOutParam, out SqlParameter statusMessageOutParam);

                await conn.OpenAsync();
                var auditLogs = await ReadAuditLogsAsync(cmd);

                return _helper.CreateOperationResult<IReadOnlyList<AuditLog>>(auditLogs, statusCodeOutParam, statusMessageOutParam);
            }
        }

        public async Task<OperationResult<IReadOnlyList<AuditLog>>> GetByEndpointUrlPagedAsync(string endpointUrl, int pageNumber, int pageSize)
        {
            using (var conn = _helper.CreateConnection())
            using (var cmd = _helper.CreateCommand(conn, "usp_AuditLogs_GetByEndpointUrlPaged"))
            {
                cmd.Parameters.Add("@EndpointUrl", SqlDbType.NVarChar, 200).Value = endpointUrl;
                cmd.Parameters.Add("@PageNumber", SqlDbType.Int).Value = pageNumber;
                cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = pageSize;
                _helper.AddDefaultParameters(cmd, out SqlParameter statusCodeOutParam, out SqlParameter statusMessageOutParam);

                await conn.OpenAsync();
                var auditLogs = await ReadAuditLogsAsync(cmd);

                return _helper.CreateOperationResult<IReadOnlyList<AuditLog>>(auditLogs, statusCodeOutParam, statusMessageOutParam);
            }
        }

        public async Task<OperationResult<IReadOnlyList<AuditLog>>> GetByHttpStatusCodePagedAsync(HttpStatusCode httpStatusCode, int pageNumber, int pageSize)
        {
            using (var conn = _helper.CreateConnection())
            using (var cmd = _helper.CreateCommand(conn, "usp_AuditLogs_GetByHttpStatusCodePaged"))
            {
                cmd.Parameters.Add("@HttpStatusCode", SqlDbType.Int).Value = (int)httpStatusCode;
                cmd.Parameters.Add("@PageNumber", SqlDbType.Int).Value = pageNumber;
                cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = pageSize;
                _helper.AddDefaultParameters(cmd, out SqlParameter statusCodeOutParam, out SqlParameter statusMessageOutParam);

                await conn.OpenAsync();
                var auditLogs = await ReadAuditLogsAsync(cmd);

                return _helper.CreateOperationResult<IReadOnlyList<AuditLog>>(auditLogs, statusCodeOutParam, statusMessageOutParam);
            }
        }

        public async Task<OperationResult<IReadOnlyList<AuditLog>>> GetByIpAddressPagedAsync(string ipAddress, int pageNumber, int pageSize)
        {
            using (var conn = _helper.CreateConnection())
            using (var cmd = _helper.CreateCommand(conn, "usp_AuditLogs_GetByIpAddressPaged"))
            {
                cmd.Parameters.Add("@IpAddress", SqlDbType.NVarChar, 50).Value = ipAddress;
                cmd.Parameters.Add("@PageNumber", SqlDbType.Int).Value = pageNumber;
                cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = pageSize;
                _helper.AddDefaultParameters(cmd, out SqlParameter statusCodeOutParam, out SqlParameter statusMessageOutParam);

                await conn.OpenAsync();
                var auditLogs = await ReadAuditLogsAsync(cmd);

                return _helper.CreateOperationResult<IReadOnlyList<AuditLog>>(auditLogs, statusCodeOutParam, statusMessageOutParam);
            }
        }

        public async Task<OperationResult<IReadOnlyList<AuditLog>>> GetCreatedBeforePagedAsync(DateTime dateTime, int pageNumber, int pageSize)
        {
            using (var conn = _helper.CreateConnection())
            using (var cmd = _helper.CreateCommand(conn, "usp_AuditLogs_GetCreatedBeforePaged"))
            {
                cmd.Parameters.Add("@CreatedBefore", SqlDbType.DateTime2).Value = dateTime;
                cmd.Parameters.Add("@PageNumber", SqlDbType.Int).Value = pageNumber;
                cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = pageSize;
                _helper.AddDefaultParameters(cmd, out SqlParameter statusCodeOutParam, out SqlParameter statusMessageOutParam);

                await conn.OpenAsync();
                var auditLogs = await ReadAuditLogsAsync(cmd);

                return _helper.CreateOperationResult<IReadOnlyList<AuditLog>>(auditLogs, statusCodeOutParam, statusMessageOutParam);
            }
        }

        public async Task<OperationResult<IReadOnlyList<AuditLog>>> GetCreatedAfterPagedAsync(DateTime dateTime, int pageNumber, int pageSize)
        {
            using (var conn = _helper.CreateConnection())
            using (var cmd = _helper.CreateCommand(conn, "usp_AuditLogs_GetCreatedAfterPaged"))
            {
                cmd.Parameters.Add("@CreatedAfter", SqlDbType.DateTime2).Value = dateTime;
                cmd.Parameters.Add("@PageNumber", SqlDbType.Int).Value = pageNumber;
                cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = pageSize;
                _helper.AddDefaultParameters(cmd, out SqlParameter statusCodeOutParam, out SqlParameter statusMessageOutParam);

                await conn.OpenAsync();
                var auditLogs = await ReadAuditLogsAsync(cmd);

                return _helper.CreateOperationResult<IReadOnlyList<AuditLog>>(auditLogs, statusCodeOutParam, statusMessageOutParam);
            }
        }


        private static AuditLog? MapAuditLog(SqlDataReader reader)
        {
            if (!reader.HasRows)
            {
                return null;
            }

            var detailsOrdinal = reader.GetOrdinal("Details");
            string? details = reader.IsDBNull(detailsOrdinal) ? null : reader.GetString(detailsOrdinal);

            var userIdOrdinal = reader.GetOrdinal("UserId");
            int? userId = reader.IsDBNull(userIdOrdinal) ? null : reader.GetInt32(userIdOrdinal);

            var attemptedLoginIdentifierOrdinal = reader.GetOrdinal("AttemptedLoginIdentifier");
            string? attemptedLoginIdentifier = reader.IsDBNull(attemptedLoginIdentifierOrdinal) ? null : reader.GetString(attemptedLoginIdentifierOrdinal);

            var requestBodyOrdinal = reader.GetOrdinal("RequestBody");
            string? requestBody = reader.IsDBNull(requestBodyOrdinal) ? null : reader.GetString(requestBodyOrdinal);

            var responseBodyOrdinal = reader.GetOrdinal("ResponseBody");
            string? responseBody = reader.IsDBNull(responseBodyOrdinal) ? null : reader.GetString(responseBodyOrdinal);

            var userAgentOrdinal = reader.GetOrdinal("UserAgent");
            string? userAgent = reader.IsDBNull(userAgentOrdinal) ? null : reader.GetString(userAgentOrdinal);

            var auditLog = new AuditLog(
                auditLogId: reader.GetInt64(reader.GetOrdinal("AuditLogId")),
                userId: userId,
                attemptedLoginIdentifier: attemptedLoginIdentifier,
                correlationId: reader.GetGuid(reader.GetOrdinal("CorrelationId")),
                actionType: (AuditActionType)reader.GetInt32(reader.GetOrdinal("ActionType")),
                endpoint: reader.GetString(reader.GetOrdinal("Endpoint")),
                requestBody: requestBody,
                responseBody: responseBody,
                userAgent: userAgent,
                statusCode: (HttpStatusCode)reader.GetInt32(reader.GetOrdinal("StatusCode")),
                isSuccess: reader.GetBoolean(reader.GetOrdinal("IsSuccess")),
                duration: reader.GetInt32(reader.GetOrdinal("Duration")),
                ipAddress: reader.GetString(reader.GetOrdinal("IpAddress")),
                details: details,
                createdAt: reader.GetDateTime(reader.GetOrdinal("CreatedAt")));

            return auditLog;
        }

        private static async Task<IReadOnlyList<AuditLog>> ReadAuditLogsAsync(SqlCommand cmd)
        {
            var auditLogs = new List<AuditLog>();

            using (var reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    auditLogs.Add(MapAuditLog(reader));
                }
            }

            return auditLogs;
        }
    }
}

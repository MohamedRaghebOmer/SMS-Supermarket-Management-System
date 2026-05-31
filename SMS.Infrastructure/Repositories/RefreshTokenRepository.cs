using Microsoft.Data.SqlClient;
using SMS.Application.Common.Results;
using SMS.Application.Interfaces.DataAccess;
using SMS.Application.Interfaces.Repositories;
using SMS.Domain.Entities;

namespace SMS.Infrastructure.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly IStoredProcedureExecutor _executor;

        public RefreshTokenRepository(IStoredProcedureExecutor executor)
        {
            _executor = executor;
        }


        public async Task<OperationResult<bool>> AddAsync(RefreshToken token)
        {
            await using var conn = _executor.CreateConnection();
            await using var cmd = _executor.CreateCommand(conn, "usp_RefreshTokens_Insert");

            AddRefreshTokenParameters(cmd, token);

            return await _executor.ExecuteNonQueryAsync(cmd, conn);
        }

        public async Task<OperationResult<bool>> IsValidRefreshTokenByUsernameAsync(Guid refreshTokenId, string username)
        {
            await using var conn = _executor.CreateConnection();
            await using var cmd = _executor.CreateCommand(conn, "usp_RefreshTokens_IsValidByUsername");

            cmd.Parameters.Add("@RefreshTokenId", System.Data.SqlDbType.UniqueIdentifier).Value = refreshTokenId;
            cmd.Parameters.Add("@Username", System.Data.SqlDbType.NVarChar, 50).Value = username;

            return await _executor.ExecuteBooleanOperationAsync(cmd, conn);
        }

        public async Task<OperationResult<bool>> RevokeAsync(Guid refreshTokenId)
        {
            await using var conn = _executor.CreateConnection();
            await using var cmd = _executor.CreateCommand(conn, "usp_RefreshTokens_Revoke");

            cmd.Parameters.Add("@RefreshTokenId", System.Data.SqlDbType.UniqueIdentifier).Value = refreshTokenId;

            return await _executor.ExecuteBooleanOperationAsync(cmd, conn);
        }

        public async Task<OperationResult<bool>> RevokeByUsernameAsync(Guid refreshTokenId, string username)
        {
            await using var conn = _executor.CreateConnection();
            await using var cmd = _executor.CreateCommand(conn, "usp_RefreshTokens_RevokeByUsername");

            cmd.Parameters.Add("@RefreshTokenId", System.Data.SqlDbType.UniqueIdentifier).Value = refreshTokenId;
            cmd.Parameters.Add("@Username", System.Data.SqlDbType.NVarChar, 50).Value = username;

            return await _executor.ExecuteBooleanOperationAsync(cmd, conn);
        }

        public async Task<OperationResult<bool>> HasValidRefreshTokenAsync(int userId)
        {
            await using var conn = _executor.CreateConnection();
            using var cmd = _executor.CreateCommand(conn, "usp_RefreshTokens_HasValidRefreshToken");

            cmd.Parameters.Add("@UserId", System.Data.SqlDbType.Int).Value = userId;

            return await _executor.ExecuteExistsAsync(conn, cmd);
        }

        public async Task<OperationResult<IReadOnlyList<RefreshToken>>> FindValidTokensByUserIdAsync(int userId)
        {
            await using var conn = _executor.CreateConnection();
            using var cmd = _executor.CreateCommand(conn, "usp_RefreshTokens_GetValidTokensByUserId");

            cmd.Parameters.Add("@UserId", System.Data.SqlDbType.Int).Value = userId;

            return await _executor.ExecuteListAsync(cmd, conn, MapToRefreshToken);
        }

        public async Task<OperationResult<IReadOnlyList<RefreshToken>>> FindValidTokensByUsernameAsync(string username)
        {
            await using var conn = _executor.CreateConnection();
            using var cmd = _executor.CreateCommand(conn, "usp_RefreshTokens_GetValidTokensByUsername");

            cmd.Parameters.Add("@Username", System.Data.SqlDbType.NVarChar, 50).Value = username;

            return await _executor.ExecuteListAsync(cmd, conn, MapToRefreshToken);
        }


        private void AddRefreshTokenParameters(SqlCommand cmd, RefreshToken token)
        {
            cmd.Parameters.Add("@RefreshTokenId", System.Data.SqlDbType.UniqueIdentifier).Value = token.RefreshTokenId;
            cmd.Parameters.Add("@UserId", System.Data.SqlDbType.Int).Value = token.UserId;
            cmd.Parameters.Add("@TokenHash", System.Data.SqlDbType.NVarChar, 500).Value = token.TokenHash;
            cmd.Parameters.Add("@ExpirationDate", System.Data.SqlDbType.DateTime2).Value = token.ExpirationDate;
        }

        private RefreshToken MapToRefreshToken(SqlDataReader reader)
        {
            var revokedAtOrdinal = reader.GetOrdinal("RevokedAt");
            DateTime? revokedAt = reader.IsDBNull(revokedAtOrdinal) ? null : reader.GetDateTime(revokedAtOrdinal);

            var isRevokedOrdinal = reader.GetOrdinal("IsRevoked");
            bool isRevoked = reader.GetBoolean(isRevokedOrdinal);

            return new RefreshToken
            {
                RefreshTokenId = reader.GetGuid(reader.GetOrdinal("RefreshTokenId")),
                UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                TokenHash = reader.GetString(reader.GetOrdinal("TokenHash")),
                ExpirationDate = reader.GetDateTime(reader.GetOrdinal("ExpirationDate")),
                CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                RevokedAt = revokedAt,
                IsRevoked = isRevoked,
            };
        }
    }
}


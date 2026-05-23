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

            AddRefeshTokenParameters(cmd, token);

            return await _executor.ExecuteNonQueryAsync(cmd, conn);
        }

        public async Task<OperationResult<bool>> IsValidRefreshTokenByUsernameAsync(string tokenHash, string username)
        {
            await using var conn = _executor.CreateConnection();
            await using var cmd = _executor.CreateCommand(conn, "usp_RefreshTokens_IsValidByUsername");

            cmd.Parameters.Add("@TokenHash", System.Data.SqlDbType.NVarChar, 500).Value = tokenHash;
            cmd.Parameters.Add("@Username", System.Data.SqlDbType.NVarChar, 50).Value = username;

            return await _executor.ExecuteBooleanOperationAsync(cmd, conn);
        }

        public async Task<OperationResult<bool>> DoesTokenBelongToUserAsync(string tokenHash, string username)
        {
            await using var conn = _executor.CreateConnection();
            await using var cmd = _executor.CreateCommand(conn, "usp_RefreshTokens_DoesTokenBelongToUser");

            cmd.Parameters.Add("@TokenHash", System.Data.SqlDbType.NVarChar, 500).Value = tokenHash;
            cmd.Parameters.Add("@Username", System.Data.SqlDbType.NVarChar, 50).Value = username;

            return await _executor.ExecuteBooleanOperationAsync(cmd, conn);
        }

        public async Task<OperationResult<bool>> RevokeAsync(string tokenHash)
        {
            await using var conn = _executor.CreateConnection();
            await using var cmd = _executor.CreateCommand(conn, "usp_RefreshTokens_Revoke");

            cmd.Parameters.Add("@TokenHash", System.Data.SqlDbType.NVarChar, 500).Value = tokenHash;

            return await _executor.ExecuteBooleanOperationAsync(cmd, conn);
        }


        private void AddRefeshTokenParameters(SqlCommand cmd, RefreshToken token)
        {
            cmd.Parameters.Add("@RefreshTokenId", System.Data.SqlDbType.UniqueIdentifier).Value = token.RefreshTokenId;
            cmd.Parameters.Add("@UserId", System.Data.SqlDbType.Int).Value = token.UserId;
            cmd.Parameters.Add("@TokenHash", System.Data.SqlDbType.NVarChar, 500).Value = token.TokenHash;
            cmd.Parameters.Add("@ExpirationDate", System.Data.SqlDbType.DateTime2).Value = token.ExpirationDate;
            cmd.Parameters.Add("@CreatedAt", System.Data.SqlDbType.DateTime2).Value = token.CreatedAt;
        }
    }
}


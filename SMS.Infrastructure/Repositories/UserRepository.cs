using Microsoft.Data.SqlClient;
using SMS.Application.Common.Results;
using SMS.Application.Interfaces.DataAccess;
using SMS.Application.Interfaces.Repositories;
using SMS.Domain.Entities;
using SMS.Shared.Common;
using System.Data;

namespace SMS.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IStoredProcedureExecutor _executor;

        public UserRepository(IStoredProcedureExecutor helper)
        {
            _executor = helper;
        }


        public async Task<OperationResult<int>> RegisterAsync(User user)
        {
            await using var conn = _executor.CreateConnection();
            using var cmd = _executor.CreateCommand(conn, "usp_Users_Insert");

            AddCommonUserParameters(cmd, user);

            var insertedIdParam = new SqlParameter("@InsertedId", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(insertedIdParam);

            return await _executor.ExecuteNonQueryAsync<int>(cmd, conn, insertedIdParam);
        }

        public async Task<OperationResult<User?>> FindByIdAsync(int userId)
        {
            await using var conn = _executor.CreateConnection();
            using var cmd = _executor.CreateCommand(conn, "usp_Users_GetById");

            cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = userId;
            return await _executor.ExecuteSingleAsync(cmd, conn, MapToUser);
        }

        public async Task<OperationResult<User?>> FindByUsernameAsync(string username)
        {
            await using var conn = _executor.CreateConnection();
            using var cmd = _executor.CreateCommand(conn, "usp_Users_GetByUsername");

            cmd.Parameters.Add("@Username", SqlDbType.NVarChar, 50).Value = username;
            return await _executor.ExecuteSingleAsync(cmd, conn, MapToUser);
        }

        /// <summary>
        /// Gets the user identifier for the specified username.
        /// </summary>
        public async Task<OperationResult<int>> GetUserIdByUsernameAsync(string username)
        {
            await using var conn = _executor.CreateConnection();
            using var cmd = _executor.CreateCommand(conn, "usp_Users_GetUserIdByUsername");

            cmd.Parameters.Add("@Username", SqlDbType.NVarChar, 50).Value = username;

            return await _executor.ExecuteScalarAsync<int>(cmd, conn);
        }

        public async Task<OperationResult<User?>> FindByPersonIdAsync(int personId)
        {
            await using var conn = _executor.CreateConnection();
            using var cmd = _executor.CreateCommand(conn, "usp_Users_GetByPersonId");

            cmd.Parameters.Add("@PersonId", SqlDbType.Int).Value = personId;
            return await _executor.ExecuteSingleAsync(cmd, conn, MapToUser);
        }

        /// <summary>
        /// Gets the user identifier for the specified person identifier.
        /// </summary>
        public async Task<OperationResult<int>> GetUserIdByPersonIdAsync(int personId)
        {
            await using var conn = _executor.CreateConnection();
            using var cmd = _executor.CreateCommand(conn, "usp_Users_GetUserIdByPersonId");

            cmd.Parameters.Add("@PersonId", SqlDbType.Int).Value = personId;

            return await _executor.ExecuteScalarAsync<int>(cmd, conn);
        }

        public async Task<OperationResult<User?>> FindByEmailAsync(string email)
        {
            await using var conn = _executor.CreateConnection();
            using var cmd = _executor.CreateCommand(conn, "usp_Users_GetByEmail");

            cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 100).Value = email;
            return await _executor.ExecuteSingleAsync(cmd, conn, MapToUser);
        }

        /// <summary>
        /// Gets the user identifier for the specified email address.
        /// </summary>
        public async Task<OperationResult<int>> GetUserIdByEmailAsync(string email)
        {
            await using var conn = _executor.CreateConnection();
            using var cmd = _executor.CreateCommand(conn, "usp_Users_GetUserIdByEmail");

            cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 100).Value = email;

            return await _executor.ExecuteScalarAsync<int>(cmd, conn);
        }

        public async Task<OperationResult<bool>> ExistsByIdAsync(int userId)
        {
            await using var conn = _executor.CreateConnection();
            using var cmd = _executor.CreateCommand(conn, "usp_Users_ExistsById");

            cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = userId;
            return await _executor.ExecuteExistsAsync(conn, cmd);
        }

        public async Task<OperationResult<bool>> ExistsByUsername(string username)
        {
            await using var conn = _executor.CreateConnection();
            using var cmd = _executor.CreateCommand(conn, "usp_Users_ExistsByUsername");

            cmd.Parameters.Add("@Username", SqlDbType.NVarChar, 50).Value = username;
            return await _executor.ExecuteExistsAsync(conn, cmd);
        }

        public async Task<OperationResult<bool>> ExistsByEmail(string email)
        {
            await using var conn = _executor.CreateConnection();
            using var cmd = _executor.CreateCommand(conn, "usp_Users_ExistsByEmail");

            cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 100).Value = email;
            return await _executor.ExecuteExistsAsync(conn, cmd);
        }

        public async Task<OperationResult<bool>> IsEmailOwnedByUserAsync(string email, int userId)
        {
            await using var conn = _executor.CreateConnection();
            using var cmd = _executor.CreateCommand(conn, "usp_Users_IsEmailOwnedByUser");

            cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 100).Value = email;
            cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = userId;

            return await _executor.ExecuteExistsAsync(conn, cmd);
        }

        public async Task<OperationResult<string>> GetPasswordHashAsync(int userId)
        {
            await using var conn = _executor.CreateConnection();
            using var cmd = _executor.CreateCommand(conn, "usp_Users_GetPasswordHash");

            cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = userId;

            return await _executor.ExecuteScalarAsync<string>(cmd, conn);
        }

        public async Task<OperationResult<PaginationResponse<User>>> GetByRoleId(int roleId, PaginationRequest paginationRequest)
        {
            await using var conn = _executor.CreateConnection();
            using var cmd = _executor.CreateCommand(conn, "usp_Users_GetByRoleId");

            cmd.Parameters.Add("@RoleId", SqlDbType.Int).Value = roleId;
            return await _executor.ExecutePaginationAsync(cmd, conn, paginationRequest, MapToUser);
        }

        public async Task<OperationResult<PaginationResponse<User>>> GetPagedAsync(PaginationRequest paginationRequest)
        {
            await using var conn = _executor.CreateConnection();
            using var cmd = _executor.CreateCommand(conn, "usp_Users_GetPaged");

            return await _executor.ExecutePaginationAsync(cmd, conn, paginationRequest, MapToUser);
        }

        public async Task<OperationResult<PaginationResponse<User>>> GetActiveUsers(PaginationRequest paginationRequest)
        {
            await using var conn = _executor.CreateConnection();
            using var cmd = _executor.CreateCommand(conn, "usp_Users_GetActiveUsers");

            return await _executor.ExecutePaginationAsync(cmd, conn, paginationRequest, MapToUser);
        }

        public async Task<OperationResult<bool>> Login(string username, string password)
        {
            await using var conn = _executor.CreateConnection();
            using var cmd = _executor.CreateCommand(conn, "usp_Users_Login");

            cmd.Parameters.Add("@Username", SqlDbType.NVarChar, 50).Value = username;
            cmd.Parameters.Add("@Password", SqlDbType.NVarChar, 100).Value = password;

            return await _executor.ExecuteBooleanOperationAsync(cmd, conn);
        }

        public async Task<OperationResult<bool>> ChangePassword(int userId,
            string newPasswordHash)
        {
            await using var conn = _executor.CreateConnection();
            using var cmd = _executor.CreateCommand(conn, "usp_Users_ChangePassword");

            cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = userId;
            cmd.Parameters.Add("@NewPasswordHash", SqlDbType.NVarChar, 256).Value = newPasswordHash;

            return await _executor.ExecuteBooleanOperationAsync(cmd, conn);
        }

        public async Task<OperationResult<bool>> ActivateAsync(int userId)
        {
            await using var conn = _executor.CreateConnection();
            using var cmd = _executor.CreateCommand(conn, "usp_Users_Activate");

            cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = userId;
            return await _executor.ExecuteBooleanOperationAsync(cmd, conn);
        }

        public async Task<OperationResult<bool>> DeactivateAsync(int userId)
        {
            await using var conn = _executor.CreateConnection();
            using var cmd = _executor.CreateCommand(conn, "usp_Users_Deactivate");

            cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = userId;
            return await _executor.ExecuteBooleanOperationAsync(cmd, conn);
        }

        public async Task<OperationResult<bool>> UpdateAsync(User user)
        {
            await using var conn = _executor.CreateConnection();
            using var cmd = _executor.CreateCommand(conn, "usp_Users_Update");

            AddCommonUserParameters(cmd, user);
            cmd.Parameters.Add("@LastUpdatedAt", SqlDbType.DateTime2).Value = user.LastUpdatedAt ?? (object)DBNull.Value;
            cmd.Parameters.Add("@IsActive", SqlDbType.Bit).Value = user.IsActive;

            return await _executor.ExecuteBooleanOperationAsync(cmd, conn);
        }

        public async Task<OperationResult<bool>> DeleteAsync(int userId)
        {
            await using var conn = _executor.CreateConnection();
            using var cmd = _executor.CreateCommand(conn, "usp_Users_Delete");

            cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = userId;
            return await _executor.ExecuteBooleanOperationAsync(cmd, conn);
        }


        private static User MapToUser(SqlDataReader reader)
        {
            var lastLoginAtOrdinal = reader.GetOrdinal("LastLoginAt");
            DateTime? lastLoginAt = reader.IsDBNull(lastLoginAtOrdinal) ? null : reader.GetDateTime(lastLoginAtOrdinal);

            var updatedAtOrdinal = reader.GetOrdinal("UpdatedAt");
            DateTime? updatedAt = reader.IsDBNull(updatedAtOrdinal) ? null : reader.GetDateTime(updatedAtOrdinal);

            return new User(
                userId: reader.GetInt32(reader.GetOrdinal("UserId")),
                personId: reader.GetInt32(reader.GetOrdinal("PersonId")),
                username: reader.GetString(reader.GetOrdinal("Username")),
                passwordHash: reader.GetString(reader.GetOrdinal("PasswordHash")),
                roleId: reader.GetInt32(reader.GetOrdinal("RoleId")),
                isActive: reader.GetBoolean(reader.GetOrdinal("IsActive")),
                lastLoginAt: lastLoginAt,
                createdAt: reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                lastUpdatedAt: updatedAt);
        }

        private static void AddCommonUserParameters(SqlCommand cmd, User user)
        {
            cmd.Parameters.Add("@PersonId", SqlDbType.Int).Value = user.PersonId;
            cmd.Parameters.Add("@Username", SqlDbType.NVarChar, 50).Value = user.Username;
            cmd.Parameters.Add("@PasswordHash", SqlDbType.NVarChar, 100).Value = user.PasswordHash;
            cmd.Parameters.Add("@RoleId", SqlDbType.Int).Value = user.RoleId;
        }
    }
}

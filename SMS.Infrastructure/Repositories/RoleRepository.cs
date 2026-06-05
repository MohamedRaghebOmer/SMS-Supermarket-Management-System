using Microsoft.Data.SqlClient;
using SMS.Application.Common.Results;
using SMS.Application.Interfaces.DataAccess;
using SMS.Application.Interfaces.Repositories;
using SMS.Domain.Entities;
using SMS.Shared.Common;
using System.Data;

namespace SMS.Infrastructure.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly IStoredProcedureExecutor _executor;

        public RoleRepository(IStoredProcedureExecutor executor)
        {
            _executor = executor;
        }

        public async Task<OperationResult<int>> AddAsync(Role role)
        {
            await using var conn = _executor.CreateConnection();
            await using var cmd = _executor.CreateCommand(conn, "usp_Roles_Insert");

            AddParameters(cmd, role, isUpdate: false);

            var insertedIdParam = new SqlParameter("@InsertedId", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(insertedIdParam);

            return await _executor.ExecuteNonQueryAsync<int>(cmd, conn, insertedIdParam);
        }

        public async Task<OperationResult<Role?>> FindByIdAsync(int roleId)
        {
            await using var conn = _executor.CreateConnection();
            await using var cmd = _executor.CreateCommand(conn, "usp_Roles_GetById");

            cmd.Parameters.Add("@RoleId", SqlDbType.Int).Value = roleId;
            return await _executor.ExecuteSingleAsync(cmd, conn, MapToRole);
        }

        public async Task<OperationResult<Role?>> FindByNameAsync(string roleName)
        {
            await using var conn = _executor.CreateConnection();
            await using var cmd = _executor.CreateCommand(conn, "usp_Roles_GetByName");

            cmd.Parameters.Add("@RoleName", SqlDbType.NVarChar, 50).Value = roleName;
            return await _executor.ExecuteSingleAsync(cmd, conn, MapToRole);
        }

        public async Task<OperationResult<string>> FindRoleNameByIdAsync(int roleId)
        {
            await using var conn = _executor.CreateConnection();
            await using var cmd = _executor.CreateCommand(conn, "usp_Roles_FindRoleNameById");

            cmd.Parameters.Add("@RoleId", System.Data.SqlDbType.Int).Value = roleId;

            var result = await _executor.ExecuteScalarAsync<string>(cmd, conn);
            if (string.IsNullOrWhiteSpace(result.Data))
            {
                result.Data = null;
            }

            return result;
        }

        public async Task<OperationResult<bool>> IsActive(int roleId)
        {
            await using var conn = _executor.CreateConnection();
            await using var cmd = _executor.CreateCommand(conn, "usp_Roles_IsActive");

            cmd.Parameters.Add("@RoleId", SqlDbType.Int).Value = roleId;
            return await _executor.ExecuteExistsAsync(conn, cmd);
        }

        public async Task<OperationResult<PaginationResponse<Role>>> GetPagedAsync(PaginationRequest request)
        {
            await using var conn = _executor.CreateConnection();
            await using var cmd = _executor.CreateCommand(conn, "usp_Roles_GetPaged");

            return await _executor.ExecutePaginationAsync(cmd, conn, request, MapToRole);
        }

        public async Task<OperationResult<PaginationResponse<Role>>> GetPagedByIsActiveAsync(PaginationRequest request, bool isActive)
        {
            await using var conn = _executor.CreateConnection();
            await using var cmd = _executor.CreateCommand(conn, "usp_Roles_GetPagedByIsActive");

            cmd.Parameters.Add("@IsActive", SqlDbType.Bit).Value = isActive;
            return await _executor.ExecutePaginationAsync(cmd, conn, request, MapToRole);
        }

        public async Task<OperationResult<PaginationResponse<Role>>> GetPagedByCreatedAtRangeAsync(PaginationRequest request, DateTime from, DateTime to)
        {
            await using var conn = _executor.CreateConnection();
            await using var cmd = _executor.CreateCommand(conn, "usp_Roles_GetPagedByCreatedAtRange");

            cmd.Parameters.Add("@From", SqlDbType.DateTime2).Value = from;
            cmd.Parameters.Add("@To", SqlDbType.DateTime2).Value = to;
            return await _executor.ExecutePaginationAsync(cmd, conn, request, MapToRole);
        }

        public async Task<OperationResult<bool>> UpdateAsync(Role role)
        {
            await using var conn = _executor.CreateConnection();
            await using var cmd = _executor.CreateCommand(conn, "usp_Roles_Update");

            AddParameters(cmd, role, isUpdate: true);
            return await _executor.ExecuteBooleanOperationAsync(cmd, conn);
        }

        public async Task<OperationResult<bool>> ActivateAsync(int roleId)
        {
            await using var conn = _executor.CreateConnection();
            await using var cmd = _executor.CreateCommand(conn, "usp_Roles_Activate");

            cmd.Parameters.Add("@RoleId", SqlDbType.Int).Value = roleId;
            return await _executor.ExecuteBooleanOperationAsync(cmd, conn);
        }

        public async Task<OperationResult<bool>> DeactivateAsync(int roleId)
        {
            await using var conn = _executor.CreateConnection();
            await using var cmd = _executor.CreateCommand(conn, "usp_Roles_Deactivate");

            cmd.Parameters.Add("@RoleId", SqlDbType.Int).Value = roleId;
            return await _executor.ExecuteBooleanOperationAsync(cmd, conn);
        }

        private static Role MapToRole(SqlDataReader reader)
        {
            var descOrdinal = reader.GetOrdinal("RoleDescription");
            string? desc = reader.IsDBNull(descOrdinal) ? null : reader.GetString(descOrdinal);

            return new Role(
                roleId: reader.GetInt32(reader.GetOrdinal("RoleId")),
                roleName: reader.GetString(reader.GetOrdinal("RoleName")),
                roleDescription: desc,
                isActive: reader.GetBoolean(reader.GetOrdinal("IsActive")));
        }

        private static void AddParameters(SqlCommand cmd, Role role, bool isUpdate)
        {
            cmd.Parameters.Add("@RoleName", SqlDbType.NVarChar, 50).Value = role.RoleName;
            cmd.Parameters.Add("@RoleDescription", SqlDbType.NVarChar, 250).Value = role.RoleDescription ?? (object)DBNull.Value;

            if (isUpdate)
            {
                cmd.Parameters.Add("@RoleId", SqlDbType.Int).Value = role.RoleId;
                cmd.Parameters.Add("@IsActive", SqlDbType.Bit).Value = role.IsActive;
            }
        }
    }
}

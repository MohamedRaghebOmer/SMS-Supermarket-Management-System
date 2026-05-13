using Microsoft.Data.SqlClient;
using SMS.Application.Common.Results;
using SMS.Application.Interfaces.DataAccess;
using SMS.Application.Interfaces.Repositories;
using SMS.Domain.Entities;
using SMS.Shared.Enums;
using System.Data;

namespace SMS.Infrastructure.Repositories
{
    public class RoleEntityPermissionsRepository : IRoleEntityPermissionRepository
    {
        private readonly IStoredProcedureExecutor _executor;

        public RoleEntityPermissionsRepository(IStoredProcedureExecutor helper)
        {
            _executor = helper;
        }


        public async Task<OperationResult<bool>> AddAsync(RoleEntityPermissions entity)
        {
            await using var conn = _executor.CreateConnection();
            using var cmd = _executor.CreateCommand(conn, "usp_RoleEntityPermissions_Insert");

            AddRoleEntityPermissionParameters(cmd, entity);

            return await _executor.ExecuteNonQueryAsync(cmd, conn);
        }

        public async Task<OperationResult<IReadOnlyList<RoleEntityPermissions>>> GetByRoleIdAsync(int roleId)
        {
            await using var conn = _executor.CreateConnection();
            using var cmd = _executor.CreateCommand(conn, "usp_RoleEntityPermissions_GetByRoleId");

            cmd.Parameters.Add("@RoleId", SqlDbType.Int).Value = roleId;

            return await _executor.ExecuteListAsync(cmd, conn, MapRoleEntityPermission);
        }

        public async Task<OperationResult<IReadOnlyList<RoleEntityPermissions>>> GetByEntityAsync(SystemEntity entity)
        {
            await using var conn = _executor.CreateConnection();
            using var cmd = _executor.CreateCommand(conn, "usp_RoleEntityPermissions_GetByEntityId");

            cmd.Parameters.Add("@EntityId", SqlDbType.Int).Value = (int)entity;

            return await _executor.ExecuteListAsync(cmd, conn, MapRoleEntityPermission);
        }

        public async Task<OperationResult<int>> GetPermissionsMaskAsync(int roleId, SystemEntity entity)
        {
            await using var conn = _executor.CreateConnection();
            using var cmd = _executor.CreateCommand(conn, "usp_RoleEntityPermissions_GetPermissionMask");

            AddRoleEntityPermissionParameters(cmd, roleId, entity);

            return await _executor.ExecuteScalarAsync<int>(cmd, conn);
        }

        public async Task<OperationResult<bool>> UpdatePermissionsMaskAsync(int roleId, SystemEntity entity, int newPermissionMask)
        {
            await using var conn = _executor.CreateConnection();
            using var cmd = _executor.CreateCommand(conn, "usp_RoleEntityPermissions_UpdatePermissionMask");

            AddRoleEntityPermissionParameters(cmd, new RoleEntityPermissions(roleId, entity, newPermissionMask));

            return await _executor.ExecuteNonQueryAsync(cmd, conn);
        }

        public async Task<OperationResult<bool>> DeleteByRoleAndEntityAsync(int roleId, SystemEntity entity)
        {
            await using var conn = _executor.CreateConnection();
            using var cmd = _executor.CreateCommand(conn, "usp_RoleEntityPermissions_DeleteByRoleAndEntity");

            AddRoleEntityPermissionParameters(cmd, roleId, entity);

            return await _executor.ExecuteNonQueryAsync(cmd, conn);
        }


        private static RoleEntityPermissions MapRoleEntityPermission(SqlDataReader reader)
        {
            return new RoleEntityPermissions(
                roleId: reader.GetInt32(reader.GetOrdinal("RoleId")),
                systemEntity: (SystemEntity)reader.GetInt32(reader.GetOrdinal("Entity")),
                permissionsMask: reader.GetInt32(reader.GetOrdinal("PermissionMask")));
        }

        private static void AddRoleEntityPermissionParameters(SqlCommand cmd, RoleEntityPermissions entity)
        {
            cmd.Parameters.Add("@RoleId", SqlDbType.Int).Value = entity.RoleId;
            cmd.Parameters.Add("@Entity", SqlDbType.Int).Value = (int)entity.Entity;
            cmd.Parameters.Add("@PermissionMask", SqlDbType.Int).Value = entity.PermissionsMask;
        }

        private static void AddRoleEntityPermissionParameters(SqlCommand cmd, int roleId, SystemEntity entity)
        {
            cmd.Parameters.Add("@RoleId", SqlDbType.Int).Value = roleId;
            cmd.Parameters.Add("@Entity", SqlDbType.Int).Value = (int)entity;
        }
    }
}

using Microsoft.Data.SqlClient;
using SMS.Application.Common.Results;
using SMS.Application.Interfaces.DataAccess;
using SMS.Application.Interfaces.Repositories;
using SMS.Domain.Entities;
using SMS.Shared.Enums;
using System.Data;

namespace SMS.Infrastructure.Repositories
{
    public class RoleEntityPermissionRepository : IRoleEntityPermissionRepository
    {
        private readonly IDataAccessHelper _helper;

        public RoleEntityPermissionRepository(IDataAccessHelper helper)
        {
            _helper = helper;
        }


        public async Task<OperationResult<bool>> AddAsync(RoleEntityPermission entity)
        {
            using (var conn = _helper.CreateConnection())
            using (var cmd = _helper.CreateCommand(conn, "usp_RoleEntityPermissions_Insert"))
            {
                cmd.Parameters.Add("@RoleId", SqlDbType.Int).Value = entity.RoleId;
                cmd.Parameters.Add("@Entity", SqlDbType.Int).Value = (int)entity.Entity;
                cmd.Parameters.Add("@PermissionMask", SqlDbType.Int).Value = entity.PermissionsMask;

                _helper.AddDefaultParameters(cmd, out SqlParameter code, out SqlParameter message);

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();

                return _helper.CreateOperationResult(code, message);
            }
        }

        public async Task<OperationResult<IReadOnlyList<RoleEntityPermission>>> GetByRoleIdAsync(int roleId)
        {
            using (var conn = _helper.CreateConnection())
            using (var cmd = _helper.CreateCommand(conn, "usp_RoleEntityPermissions_GetByRoleId"))
            {
                cmd.Parameters.Add("@RoleId", SqlDbType.Int).Value = roleId;
                _helper.AddDefaultParameters(cmd, out SqlParameter code, out SqlParameter message);

                await conn.OpenAsync();
                var permissions = await ReadRoleEntityPermissionsAsync(cmd);

                return _helper.CreateOperationResult<IReadOnlyList<RoleEntityPermission>>(permissions, code, message);
            }
        }

        public async Task<OperationResult<IReadOnlyList<RoleEntityPermission>>> GetByEntityAsync(SystemEntity entity)
        {
            using (var conn = _helper.CreateConnection())
            using (var cmd = _helper.CreateCommand(conn, "usp_RoleEntityPermissions_GetByEntity"))
            {
                cmd.Parameters.Add("@Entity", SqlDbType.Int).Value = (int)entity;
                _helper.AddDefaultParameters(cmd, out SqlParameter code, out SqlParameter message);

                await conn.OpenAsync();
                var permissions = await ReadRoleEntityPermissionsAsync(cmd);

                return _helper.CreateOperationResult<IReadOnlyList<RoleEntityPermission>>(permissions, code, message);
            }
        }

        public async Task<OperationResult<int>> GetPermissionsMaskAsync(int roleId, SystemEntity entity)
        {
            using (var conn = _helper.CreateConnection())
            using (var cmd = _helper.CreateCommand(conn, "usp_RoleEntityPermissions_GetPermissionMask"))
            {
                cmd.Parameters.Add("@RoleId", SqlDbType.Int).Value = roleId;
                cmd.Parameters.Add("@Entity", SqlDbType.Int).Value = (int)entity;
                _helper.AddDefaultParameters(cmd, out SqlParameter code, out SqlParameter message);

                await conn.OpenAsync();
                return _helper.CreateOperationResult((int)await cmd.ExecuteScalarAsync(), code, message);
            }
        }

        public async Task<OperationResult<bool>> UpdatePermissionsMaskAsync(int roleId, SystemEntity entity, int permissionMask)
        {
            using (var conn = _helper.CreateConnection())
            using (var cmd = _helper.CreateCommand(conn, "usp_RoleEntityPermissions_UpdatePermissionMask"))
            {
                cmd.Parameters.Add("@RoleId", SqlDbType.Int).Value = roleId;
                cmd.Parameters.Add("@Entity", SqlDbType.Int).Value = (int)entity;
                cmd.Parameters.Add("@PermissionMask", SqlDbType.Int).Value = permissionMask;
                _helper.AddDefaultParameters(cmd, out SqlParameter code, out SqlParameter message);

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();

                return _helper.CreateOperationResult(code, message);
            }
        }

        public async Task<OperationResult<bool>> DeleteByRoleIdAsync(int roleId)
        {
            using (var conn = _helper.CreateConnection())
            using (var cmd = _helper.CreateCommand(conn, "usp_RoleEntityPermissions_DeleteByRoleId"))
            {
                cmd.Parameters.Add("@RoleId", SqlDbType.Int).Value = roleId;
                _helper.AddDefaultParameters(cmd, out SqlParameter code, out SqlParameter message);

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();

                return _helper.CreateOperationResult(code, message);
            }
        }

        public async Task<OperationResult<bool>> DeleteByRoleAndEntityAsync(int roleId, SystemEntity entity)
        {
            using (var conn = _helper.CreateConnection())
            using (var cmd = _helper.CreateCommand(conn, "usp_RoleEntityPermissions_DeleteByRoleAndEntity"))
            {
                cmd.Parameters.Add("@RoleId", SqlDbType.Int).Value = roleId;
                cmd.Parameters.Add("@Entity", SqlDbType.Int).Value = (int)entity;
                _helper.AddDefaultParameters(cmd, out SqlParameter code, out SqlParameter message);

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();

                return _helper.CreateOperationResult(code, message);
            }
        }


        private static RoleEntityPermission MapRoleEntityPermission(SqlDataReader reader)
        {
            return new RoleEntityPermission(
                roleId: reader.GetInt32(reader.GetOrdinal("RoleId")),
                systemEntity: (SystemEntity)reader.GetInt32(reader.GetOrdinal("Entity")),
                permissionsMask: reader.GetInt32(reader.GetOrdinal("PermissionMask")));
        }

        private static async Task<IReadOnlyList<RoleEntityPermission>> ReadRoleEntityPermissionsAsync(SqlCommand cmd)
        {
            var permissions = new List<RoleEntityPermission>();

            using (var reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    permissions.Add(MapRoleEntityPermission(reader));
                }
            }

            return permissions;
        }
    }
}

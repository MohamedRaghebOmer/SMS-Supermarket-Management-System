using SMS.Core;
using SMS.Core.DTOs;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace SMS.Repository
{
    public class RoleRepository : IRepository<Role>
    {
        public async Task<DBResponse<int>> AddAsync(Role role)
        {
            using (var conn = Helper.CreateConnection())
            using (var cmd = Helper.CreateCommand(conn, "uspRoles_Create"))
            {
                cmd.Parameters.Add("@RoleName", SqlDbType.NVarChar, 50).Value = role.RoleName;
                cmd.Parameters.Add("@RoleDescription", SqlDbType.NVarChar, 250).Value =
                    string.IsNullOrWhiteSpace(role.RoleDescription) ? (object)DBNull.Value : role.RoleDescription;
                cmd.Parameters.Add("@IsActive", SqlDbType.Bit).Value = role.IsActive;

                SqlParameter newIdOutParam = new SqlParameter("@NewId", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(newIdOutParam);

                Helper.AddStatusParams(cmd, out SqlParameter code, out SqlParameter message);

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();

                role.RoleId = (int)newIdOutParam.Value;
                role.Mode = Core.Enums.EntityMode.Update;

                return Helper.CreateDBResponse<int>(newIdOutParam, code, message);
            }
        }

        public async Task<DBResponse<Role>> GetAsync(int id)
        {
            using (var conn = Helper.CreateConnection())
            using (var cmd = Helper.CreateCommand(conn, "uspRoles_GetById"))
            {
                cmd.Parameters.Add("@RoleId", SqlDbType.Int).Value = id;
                Helper.AddStatusParams(cmd, out SqlParameter code, out SqlParameter message);

                await conn.OpenAsync();

                Role role = null;
                using (SqlDataReader reader = await cmd.ExecuteReaderAsync(CommandBehavior.SingleRow))
                {
                    if (await reader.ReadAsync())
                    {
                        role = new Role
                        (
                            roleId: reader.GetInt32(reader.GetOrdinal("RoleId")),
                            roleName: reader.GetString(reader.GetOrdinal("RoleName")),
                            roleDescription: reader.GetString(reader.GetOrdinal("RoleDescription")),
                            isActive: reader.GetBoolean(reader.GetOrdinal("IsActive")),
                            createdAt: reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                            mode: Core.Enums.EntityMode.Update
                        );
                    }
                }

                return Helper.CreateDBResponse(role, code, message);
            }
        }

        public async Task<DBResponse<bool>> ExistsAsync(int id)
        {
            using (var conn = Helper.CreateConnection())
            using (var cmd = Helper.CreateCommand(conn, "uspRoles_ExistsById"))
            {
                cmd.Parameters.Add("@RoleId", SqlDbType.Int).Value = id;
                Helper.AddStatusParams(cmd, out SqlParameter code, out SqlParameter message);

                await conn.OpenAsync();
                return Helper.CreateDBResponse(await cmd.ExecuteScalarAsync() != null, code, message);
            }
        }

        public async Task<DBResponse<DataTable>> GetAllAsync()
        {
            using (var conn = Helper.CreateConnection())
            using (var cmd = Helper.CreateCommand(conn, "uspRoles_GetAll"))
            {
                Helper.AddStatusParams(cmd, out SqlParameter code, out SqlParameter message);

                await conn.OpenAsync();
                return Helper.CreateDBResponse(await Helper.ExecuteDataTableAsync(cmd), code, message);
            }
        }

        public async Task<DBResponse<bool>> UpdateAsync(Role role)
        {
            using (var conn = Helper.CreateConnection())
            using (var cmd = Helper.CreateCommand(conn, "uspRoles_Update"))
            {
                cmd.Parameters.Add("@RoleId", SqlDbType.Int).Value = role.RoleId;
                cmd.Parameters.Add("@RoleName", SqlDbType.NVarChar, 50).Value = role.RoleName;
                cmd.Parameters.Add("@RoleDescription", SqlDbType.NVarChar, 200).Value =
                    string.IsNullOrWhiteSpace(role.RoleDescription) ? (object)DBNull.Value : role.RoleDescription;
                cmd.Parameters.Add("@IsActive", SqlDbType.Bit).Value = role.IsActive;

                Helper.AddStatusParams(cmd, out SqlParameter code, out SqlParameter message);

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();

                return Helper.CreateDBResponse(code, message);
            }
        }

        public async Task<DBResponse<bool>> DeleteAsync(int id)
        {
            using (var conn = Helper.CreateConnection())
            using (var cmd = Helper.CreateCommand(conn, "uspRoles_Delete"))
            {
                cmd.Parameters.Add("@RoleId", SqlDbType.Int).Value = id;
                Helper.AddStatusParams(cmd, out SqlParameter code, out SqlParameter message);

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();

                return Helper.CreateDBResponse(code, message);
            }
        }
    }
}

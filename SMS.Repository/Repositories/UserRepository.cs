using SMS.Core;
using SMS.Core.DTOs;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using SMS.Core.Interfaces;

namespace SMS.Repository
{
    public class UserRepository : IRepository<User>
    {
        public async Task<DBResponse<int>> AddAsync(User user)
        {
            using (var conn = Helper.CreateConnection())
            using (var cmd = Helper.CreateCommand(conn, "uspUsers_Create"))
            {
                cmd.Parameters.Add("@PersonId", SqlDbType.Int).Value = user.PersonId;
                cmd.Parameters.Add("@Username", SqlDbType.NVarChar, 50).Value = user.Username;
                cmd.Parameters.Add("@PasswordHash", SqlDbType.NVarChar, 255).Value = user.PasswordHash;
                cmd.Parameters.Add("@RoleId", SqlDbType.Int).Value = user.RoleId;
                cmd.Parameters.Add("@IsActive", SqlDbType.Bit).Value = user.IsActive;

                SqlParameter newIdOutParam = new SqlParameter("@NewId", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(newIdOutParam);

                Helper.AddDefaultParameters(cmd, out SqlParameter code, out SqlParameter message);

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();

                user.UserId = (int)newIdOutParam.Value;

                return Helper.CreateDBResponse<int>(newIdOutParam, code, message);
            }
        }

        public async Task<DBResponse<User>> GetAsync(int id)
        {
            using (var conn = Helper.CreateConnection())
            using (var cmd = Helper.CreateCommand(conn, "uspUsers_GetById"))
            {
                cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = id;
                Helper.AddDefaultParameters(cmd, out SqlParameter code, out SqlParameter message);

                await conn.OpenAsync();

                User user = null;
                using (SqlDataReader reader = await cmd.ExecuteReaderAsync(CommandBehavior.SingleRow))
                {
                    if (await reader.ReadAsync())
                    {
                        int userId = reader.GetInt32(reader.GetOrdinal("UserId"));
                        int personId = reader.GetInt32(reader.GetOrdinal("PersonId"));
                        string username = reader.GetString(reader.GetOrdinal("Username"));
                        string passwordHash = reader.GetString(reader.GetOrdinal("PasswordHash"));

                        int tokenHashOrdinal = reader.GetOrdinal("TokenHash");
                        string tokenHash = reader.IsDBNull(tokenHashOrdinal) ? null : reader.GetString(tokenHashOrdinal);

                        int roleId = reader.GetInt32(reader.GetOrdinal("RoleId"));
                        bool isActive = reader.GetBoolean(reader.GetOrdinal("IsActive"));

                        int lastLoginAtOrdinal = reader.GetOrdinal("LastLoginAt");
                        DateTime lastLoginAt = reader.IsDBNull(lastLoginAtOrdinal)
                            ? default(DateTime)
                            : reader.GetDateTime(lastLoginAtOrdinal);

                        DateTime createdAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt"));
                        DateTime updatedAt = reader.GetDateTime(reader.GetOrdinal("UpdatedAt"));

                        user = new User(
                            userId: userId,
                            personId: personId,
                            person: null,
                            username: username,
                            passwordHash: passwordHash,
                            tokenHash: tokenHash,
                            roleId: roleId,
                            role: null,
                            isActive: isActive,
                            lastLoginAt: lastLoginAt,
                            createdAt: createdAt,
                            updatedAt: updatedAt);
                    }
                }

                return Helper.CreateDBResponse(user, code, message);
            }
        }

        public async Task<DBResponse<bool>> ExistsAsync(int id)
        {
            using (var conn = Helper.CreateConnection())
            using (var cmd = Helper.CreateCommand(conn, "uspUsers_ExistsById"))
            {
                cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = id;
                Helper.AddDefaultParameters(cmd, out SqlParameter code, out SqlParameter message);

                await conn.OpenAsync();
                return Helper.CreateDBResponse(await cmd.ExecuteScalarAsync() != null, code, message);
            }
        }

        public async Task<DBResponse<DataTable>> GetAllAsync()
        {
            using (var conn = Helper.CreateConnection())
            using (var cmd = Helper.CreateCommand(conn, "uspUsers_GetAll"))
            {
                Helper.AddDefaultParameters(cmd, out SqlParameter code, out SqlParameter message);

                await conn.OpenAsync();
                return Helper.CreateDBResponse(await Helper.ExecuteDataTableAsync(cmd), code, message);
            }
        }

        public async Task<DBResponse<bool>> UpdateAsync(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            using (var conn = Helper.CreateConnection())
            using (var cmd = Helper.CreateCommand(conn, "uspUsers_Update"))
            {
                cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = user.UserId;
                cmd.Parameters.Add("@Username", SqlDbType.NVarChar, 50).Value = user.Username;
                cmd.Parameters.Add("@PasswordHash", SqlDbType.NVarChar, 255).Value = user.PasswordHash;
                cmd.Parameters.Add("@TokenHash", SqlDbType.NVarChar, 255).Value = (object)user.TokenHash ?? DBNull.Value;
                cmd.Parameters.Add("@RoleId", SqlDbType.Int).Value = user.RoleId;
                cmd.Parameters.Add("@IsActive", SqlDbType.Bit).Value = user.IsActive;
                cmd.Parameters.Add("@LastLoginAt", SqlDbType.DateTime2).Value =
                    user.LastLoginAt == default(DateTime) ? (object)DBNull.Value : user.LastLoginAt;

                Helper.AddDefaultParameters(cmd, out SqlParameter code, out SqlParameter message);

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();

                return Helper.CreateDBResponse(code, message);
            }
        }

        public async Task<DBResponse<bool>> DeleteAsync(int id)
        {
            using (var conn = Helper.CreateConnection())
            using (var cmd = Helper.CreateCommand(conn, "uspUsers_Delete"))
            {
                cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = id;
                Helper.AddDefaultParameters(cmd, out SqlParameter code, out SqlParameter message);

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();

                return Helper.CreateDBResponse(code, message);
            }
        }

        public async Task<DBResponse<bool>> RegisterLogin(int userId)
        {
            using (var conn = Helper.CreateConnection())
            using (var cmd = Helper.CreateCommand(conn, "uspUsers_RegisterLogin"))
            {
                cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = userId;
                Helper.AddDefaultParameters(cmd, out SqlParameter code, out SqlParameter message);

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();

                return Helper.CreateDBResponse(code, message);
            }
        }

        public async Task<DBResponse<bool>> CanLogin(string username, string passwordHash)
        {
            using (var conn = Helper.CreateConnection())
            using (var cmd = Helper.CreateCommand(conn, "uspUsers_CanLogin"))
            {
                cmd.Parameters.Add("@Username", SqlDbType.NVarChar, 50).Value = username;
                cmd.Parameters.Add("@PasswordHash", SqlDbType.NVarChar, 255).Value = passwordHash;

                SqlParameter canLoginOutParam = new SqlParameter("@CanLogin", SqlDbType.Bit)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(canLoginOutParam);

                Helper.AddDefaultParameters(cmd, out SqlParameter code, out SqlParameter message);

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();

                bool canLogin = canLoginOutParam.Value != DBNull.Value && (bool)canLoginOutParam.Value;
                return Helper.CreateDBResponse(canLogin, code, message);
            }
        }

        public async Task<DBResponse<bool>> UpdateTokenHash(int userId, string tokenHash)
        {
            using (var conn = Helper.CreateConnection())
            using (var cmd = Helper.CreateCommand(conn, "uspUsers_UpdateTokenHash"))
            {
                cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = userId;
                cmd.Parameters.Add("@TokenHash", SqlDbType.NVarChar, 255).Value = (object)tokenHash ?? DBNull.Value;
                Helper.AddDefaultParameters(cmd, out SqlParameter code, out SqlParameter message);

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();

                return Helper.CreateDBResponse(code, message);
            }
        }
    }
}

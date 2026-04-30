using SMS.Core;
using SMS.Core.DTOs;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using SMS.Core.Interfaces;

namespace SMS.Repository
{
    public class CountryRepository : IRepository<Country>
    {
        public async Task<DBResponse<int>> AddAsync(Country country)
        {
            using (var conn = Helper.CreateConnection())
            using (var cmd = Helper.CreateCommand(conn, "usp_Countries_Insert"))
            {
                cmd.Parameters.Add("@CountryName", SqlDbType.NVarChar, 50).Value = country.CountryName;

                SqlParameter newIdOutParam = new SqlParameter("@NewId", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(newIdOutParam);

                // User and IP tracking parameters is added in case of insert, update and delete operations
                Helper.AddDefaultParameters(cmd, out SqlParameter code, out SqlParameter message, addUserAndIp: true);

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();

                return Helper.CreateDBResponse<int>(newIdOutParam, code, message);
            }
        }

        public async Task<DBResponse<Country>> GetAsync(int countryId)
        {
            using (SqlConnection conn = Helper.CreateConnection())
            using (SqlCommand cmd = Helper.CreateCommand(conn, "usp_Countries_GetById"))
            {
                // Input Parameters
                cmd.Parameters.Add("@CountryId", SqlDbType.Int).Value = countryId;
                Helper.AddDefaultParameters(cmd, out SqlParameter code, out SqlParameter message, false);

                await conn.OpenAsync();

                Country country = null;
                using (SqlDataReader reader = await cmd.ExecuteReaderAsync(CommandBehavior.SingleRow))
                {
                    if (await reader.ReadAsync())
                    {
                        country = new Country
                        (
                            countryId: reader.GetInt32(reader.GetOrdinal("CountryId")),
                            countryName: reader.GetString(reader.GetOrdinal("CountryName"))
                        );
                    }
                }

                return Helper.CreateDBResponse<Country>(country, code, message);
            }
        }

        public async Task<DBResponse<Country>> GetByNameAsync(string countryName)
        {
            using (SqlConnection conn = Helper.CreateConnection())
            using (SqlCommand cmd = Helper.CreateCommand(conn, "usp_Countries_GetByName"))
            {
                // Input Parameters
                cmd.Parameters.Add("@CountryName", SqlDbType.NVarChar, 50).Value = countryName;
                Helper.AddDefaultParameters(cmd, out SqlParameter code, out SqlParameter message);

                await conn.OpenAsync();

                Country country = null;
                using (SqlDataReader reader = await cmd.ExecuteReaderAsync(CommandBehavior.SingleRow))
                {
                    if (await reader.ReadAsync())
                    {
                        country = new Country
                        (
                            reader.GetInt32(reader.GetOrdinal("CountryId")),
                            reader.GetString(reader.GetOrdinal("CountryName"))
                        );
                    }
                }

                return Helper.CreateDBResponse<Country>(country, code, message);
            }
        }

        public async Task<DBResponse<bool>> ExistsAsync(int countryId)
        {
            using (SqlConnection conn = Helper.CreateConnection())
            using (SqlCommand cmd = Helper.CreateCommand(conn, "usp_Countries_ExistsById"))
            {
                // Input Parameters
                cmd.Parameters.Add("@CountryId", SqlDbType.Int).Value = countryId;
                Helper.AddDefaultParameters(cmd, out SqlParameter code, out SqlParameter message);

                await conn.OpenAsync();
                object result = await cmd.ExecuteScalarAsync();
                bool exists = result != null;

                return Helper.CreateDBResponse<bool>(exists, code, message);
            }
        }

        public async Task<DBResponse<DataTable>> GetAllAsync()
        {
            using (var conn = Helper.CreateConnection())
            using (var cmd = Helper.CreateCommand(conn, "usp_Countries_GetAll"))
            {
                Helper.AddDefaultParameters(cmd, out SqlParameter code, out SqlParameter message);

                await conn.OpenAsync();
                DataTable dtCountries = await Helper.ExecuteDataTableAsync(cmd);

                return Helper.CreateDBResponse<DataTable>(dtCountries, code, message);
            }
        }

        public async Task<DBResponse<DataTable>> GetPagedAsync(int pageSize, int? lastCountryId)
        {
            using (var conn = Helper.CreateConnection())
            using (var cmd = Helper.CreateCommand(conn, "usp_Countries_GetPaged"))
            {
                cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = pageSize;
                cmd.Parameters.Add("@LastCountryId", SqlDbType.Int).Value = (object)lastCountryId ?? DBNull.Value;
                Helper.AddDefaultParameters(cmd, out SqlParameter statusCodeOutParam, out SqlParameter statusMessageOutParam);

                await conn.OpenAsync();
                DataTable dtCountries = await Helper.ExecuteDataTableAsync(cmd);

                return Helper.CreateDBResponse<DataTable>(dtCountries, statusCodeOutParam, statusMessageOutParam);
            }
        }

        public async Task<DBResponse<bool>> UpdateAsync(Country country)
        {
            using (var conn = Helper.CreateConnection())
            using (var cmd = Helper.CreateCommand(conn, "usp_Countries_Update"))
            {
                cmd.Parameters.Add("@CountryId", SqlDbType.Int).Value = country.CountryId;
                cmd.Parameters.Add("@CountryName", SqlDbType.NVarChar, 50).Value = country.CountryName;

                // User and IP tracking parameters is added in case of insert, update and delete operations
                Helper.AddDefaultParameters(cmd, out SqlParameter code, out SqlParameter message, addUserAndIp: true);

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();

                return Helper.CreateDBResponse(code, message);
            }
        }

        public async Task<DBResponse<bool>> DeleteAsync(int countryId)
        {
            using (var conn = Helper.CreateConnection())
            using (var cmd = Helper.CreateCommand(conn, "usp_Countries_Delete"))
            {
                cmd.Parameters.Add("@CountryId", SqlDbType.Int).Value = countryId;

                // User and IP tracking parameters is added in case of insert, update and delete operations
                Helper.AddDefaultParameters(cmd, out SqlParameter code, out SqlParameter message, addUserAndIp: true);

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();

                return Helper.CreateDBResponse(code, message);
            }
        }
    }
}
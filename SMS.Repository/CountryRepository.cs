using SMS.Core;
using SMS.Core.DTOs;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace SMS.Repository
{
    public class CountryRepository : IRepository<Country>
    {
        public async Task<DBResponse<int>> AddAsync(Country country)
        {
            using (var conn = Helper.CreateConnection())
            using (var cmd = Helper.CreateCommand(conn, "uspCountries_Create"))
            {
                cmd.Parameters.Add("@CountryName", SqlDbType.NVarChar, 50).Value = country.CountryName;

                SqlParameter newIdOutParam = new SqlParameter("@NewId", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(newIdOutParam);

                Helper.AddStatusParams(cmd, out SqlParameter code, out SqlParameter message);

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();

                // If the operation was successful, assign the new ID back to the country object
                country.CountryId = (int)newIdOutParam.Value;

                return Helper.CreateDBResponse<int>(newIdOutParam, code, message);
            }
        }

        public async Task<DBResponse<Country>> GetAsync(int countryId)
        {
            using (SqlConnection conn = Helper.CreateConnection())
            using (SqlCommand cmd = Helper.CreateCommand(conn, "uspCountries_GetById"))
            {
                // Input Parameters
                cmd.Parameters.Add("@CountryId", SqlDbType.Int).Value = countryId;
                Helper.AddStatusParams(cmd, out SqlParameter code, out SqlParameter message);

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
            using (SqlCommand cmd = Helper.CreateCommand(conn, "uspCountries_GetByName"))
            {
                // Input Parameters
                cmd.Parameters.Add("@CountryName", SqlDbType.NVarChar, 50).Value = countryName;
                Helper.AddStatusParams(cmd, out SqlParameter code, out SqlParameter message);

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

        public async Task<DBResponse<DataTable>> GetAllAsync()
        {
            using (var conn = Helper.CreateConnection())
            using (var cmd = Helper.CreateCommand(conn, "uspCountries_GetAll"))
            {
                Helper.AddStatusParams(cmd, out SqlParameter code, out SqlParameter message);

                await conn.OpenAsync();
                DataTable dtCountries = await Helper.ExecuteDataTableAsync(cmd);

                return Helper.CreateDBResponse<DataTable>(dtCountries, code, message);
            }
        }

        public async Task<DBResponse<DataTable>> GetPagedAsync(int pageSize, int? lastCountryId)
        {
            using (var conn = Helper.CreateConnection())
            using (var cmd = Helper.CreateCommand(conn, "uspCountries_GetPaged"))
            {
                cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = pageSize;
                cmd.Parameters.Add("@LastCountryId", SqlDbType.Int).Value = (object)lastCountryId ?? DBNull.Value;
                Helper.AddStatusParams(cmd, out SqlParameter statusCodeOutParam, out SqlParameter statusMessageOutParam);

                await conn.OpenAsync();
                DataTable dtCountries = await Helper.ExecuteDataTableAsync(cmd);

                return Helper.CreateDBResponse<DataTable>(dtCountries, statusCodeOutParam, statusMessageOutParam);
            }
        }

        public async Task<DBResponse<bool>> UpdateAsync(Country country)
        {
            using (var conn = Helper.CreateConnection())
            using (var cmd = Helper.CreateCommand(conn, "uspCountries_Update"))
            {
                cmd.Parameters.Add("@CountryId", SqlDbType.Int).Value = country.CountryId;
                cmd.Parameters.Add("@CountryName", SqlDbType.NVarChar, 50).Value = country.CountryName;

                Helper.AddStatusParams(cmd, out SqlParameter code, out SqlParameter message);

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();

                return Helper.CreateDBResponse(code, message);
            }
        }

        public async Task<DBResponse<bool>> DeleteAsync(int countryId)
        {
            using (var conn = Helper.CreateConnection())
            using (var cmd = Helper.CreateCommand(conn, "uspCountries_Delete"))
            {
                cmd.Parameters.Add("@CountryId", SqlDbType.Int).Value = countryId;
                Helper.AddStatusParams(cmd, out SqlParameter code, out SqlParameter message);

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();

                return Helper.CreateDBResponse(code, message);
            }
        }
    }
}
using Microsoft.Data.SqlClient;
using SMS.Application.Common.Results;
using SMS.Application.Interfaces.DataAccess;
using SMS.Application.Interfaces.Repositories;
using SMS.Domain.Entities;
using System.Data;

namespace SMS.Infrastructure.Repositories
{
    public class CountryRepository : ICountryRepository
    {
        private readonly IDataAccessHelper _helper;

        public CountryRepository(IDataAccessHelper helper)
        {
            _helper = helper;
        }


        public async Task<OperationResult<int>> AddAsync(Country country)
        {
            using (var conn = _helper.CreateConnection())
            using (var cmd = _helper.CreateCommand(conn, "usp_Countries_Insert"))
            {
                cmd.Parameters.Add("@CountryName", SqlDbType.NVarChar, 50).Value = country.CountryName;

                SqlParameter insertedIdParam = new SqlParameter("@InsertedId", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(insertedIdParam);

                _helper.AddDefaultParameters(cmd, out SqlParameter code, out SqlParameter message);

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();

                return _helper.CreateOperationResult((int)insertedIdParam.Value, code, message);
            }
        }

        public async Task<OperationResult<Country?>> GetAsync(int countryId)
        {
            using (SqlConnection conn = _helper.CreateConnection())
            using (SqlCommand cmd = _helper.CreateCommand(conn, "usp_Countries_GetById"))
            {
                cmd.Parameters.Add("@CountryId", SqlDbType.Int).Value = countryId;
                _helper.AddDefaultParameters(cmd, out SqlParameter code, out SqlParameter message);

                await conn.OpenAsync();

                Country? country = MapToCountry(await cmd.ExecuteReaderAsync(CommandBehavior.SingleRow));

                return _helper.CreateOperationResult<Country>(country, code, message);
            }
        }

        public async Task<OperationResult<Country?>> GetByNameAsync(string countryName)
        {
            using (SqlConnection conn = _helper.CreateConnection())
            using (SqlCommand cmd = _helper.CreateCommand(conn, "usp_Countries_GetByName"))
            {
                // Input Parameters
                cmd.Parameters.Add("@CountryName", SqlDbType.NVarChar, 50).Value = countryName;
                _helper.AddDefaultParameters(cmd, out SqlParameter code, out SqlParameter message);

                await conn.OpenAsync();

                Country country = MapToCountry(await cmd.ExecuteReaderAsync(CommandBehavior.SingleRow));

                return _helper.CreateOperationResult<Country>(country, code, message);
            }
        }

        public async Task<OperationResult<bool>> ExistsAsync(int countryId)
        {
            using (SqlConnection conn = _helper.CreateConnection())
            using (SqlCommand cmd = _helper.CreateCommand(conn, "usp_Countries_ExistsById"))
            {
                cmd.Parameters.Add("@CountryId", SqlDbType.Int).Value = countryId;
                _helper.AddDefaultParameters(cmd, out SqlParameter code, out SqlParameter message);

                await conn.OpenAsync();
                return _helper.CreateOperationResult(await cmd.ExecuteScalarAsync() != null, code, message);
            }
        }

        public async Task<OperationResult<bool>> ExistsAsync(string countryName)
        {
            using (SqlConnection conn = _helper.CreateConnection())
            using (SqlCommand cmd = _helper.CreateCommand(conn, "usp_Countries_ExistsByName"))
            {
                cmd.Parameters.Add("@CountryName", SqlDbType.NVarChar, 50).Value = countryName;
                _helper.AddDefaultParameters(cmd, out SqlParameter code, out SqlParameter message);

                await conn.OpenAsync();
                return _helper.CreateOperationResult(await cmd.ExecuteScalarAsync() != null, code, message);
            }
        }

        public async Task<OperationResult<IReadOnlyList<Country>>> GetAllAsync()
        {
            using (var conn = _helper.CreateConnection())
            using (var cmd = _helper.CreateCommand(conn, "usp_Countries_GetAll"))
            {
                _helper.AddDefaultParameters(cmd, out SqlParameter code, out SqlParameter message);

                await conn.OpenAsync();
                var countries = await ReadCountriesAsync(cmd);

                return _helper.CreateOperationResult<IReadOnlyList<Country>>(countries, code, message);
            }
        }

        public async Task<OperationResult<IReadOnlyList<Country>>> GetPagedAsync(int pageSize, int? lastCountryId)
        {
            using (var conn = _helper.CreateConnection())
            using (var cmd = _helper.CreateCommand(conn, "usp_Countries_GetPaged"))
            {
                cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = pageSize;
                cmd.Parameters.Add("@LastCountryId", SqlDbType.Int).Value = lastCountryId ?? (object)DBNull.Value;
                _helper.AddDefaultParameters(cmd, out SqlParameter statusCodeOutParam, out SqlParameter statusMessageOutParam);

                await conn.OpenAsync();
                var countries = await ReadCountriesAsync(cmd);

                return _helper.CreateOperationResult<IReadOnlyList<Country>>(countries, statusCodeOutParam, statusMessageOutParam);
            }
        }

        public async Task<OperationResult<bool>> UpdateAsync(Country country)
        {
            using (var conn = _helper.CreateConnection())
            using (var cmd = _helper.CreateCommand(conn, "usp_Countries_Update"))
            {
                cmd.Parameters.Add("@CountryId", SqlDbType.Int).Value = country.CountryId;
                cmd.Parameters.Add("@CountryName", SqlDbType.NVarChar, 50).Value = country.CountryName;
                _helper.AddDefaultParameters(cmd, out SqlParameter code, out SqlParameter message);

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();

                return _helper.CreateOperationResult(code, message);
            }
        }

        public async Task<OperationResult<bool>> DeleteAsync(int countryId)
        {
            using (var conn = _helper.CreateConnection())
            using (var cmd = _helper.CreateCommand(conn, "usp_Countries_Delete"))
            {
                cmd.Parameters.Add("@CountryId", SqlDbType.Int).Value = countryId;

                _helper.AddDefaultParameters(cmd, out SqlParameter code, out SqlParameter message);

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();

                return _helper.CreateOperationResult(code, message);
            }
        }


        private Country MapToCountry(SqlDataReader reader)
        {
            return new Country
            (
                countryId: reader.GetInt32(reader.GetOrdinal("CountryId")),
                countryName: reader.GetString(reader.GetOrdinal("CountryName"))
            );
        }

        private async Task<IReadOnlyList<Country>> ReadCountriesAsync(SqlCommand cmd)
        {
            var countries = new List<Country>();

            using (var reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    countries.Add(MapToCountry(reader));
                }
            }

            return countries;
        }
    }
}

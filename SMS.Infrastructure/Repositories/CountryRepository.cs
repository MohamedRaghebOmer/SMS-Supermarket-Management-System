using Microsoft.Data.SqlClient;
using SMS.Application.Common.Results;
using SMS.Application.Interfaces.DataAccess;
using SMS.Application.Interfaces.Repositories;
using SMS.Domain.Entities;
using SMS.Shared.Common;
using System.Data;

namespace SMS.Infrastructure.Repositories
{
    public class CountryRepository : ICountryRepository
    {
        private readonly IStoredProcedureExecutor _executor;

        public CountryRepository(IStoredProcedureExecutor helper)
        {
            _executor = helper;
        }


        public async Task<OperationResult<int>> AddAsync(Country country)
        {
            await using var conn = _executor.CreateConnection();
            using var cmd = _executor.CreateCommand(conn, "usp_Countries_Insert");

            AddCountryParameters(cmd, country);

            SqlParameter insertedIdParam = new SqlParameter("@InsertedId", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(insertedIdParam);

            return await _executor.ExecuteNonQueryAsync(cmd, conn, (int)insertedIdParam.Value);
        }

        public async Task<OperationResult<Country?>> FindAsync(int countryId)
        {
            await using var conn = _executor.CreateConnection();
            using var cmd = _executor.CreateCommand(conn, "usp_Countries_GetById");

            cmd.Parameters.Add("@CountryId", SqlDbType.Int).Value = countryId;
            return await _executor.ExecuteSingleAsync(cmd, conn, MapToCountry);
        }

        public async Task<OperationResult<Country?>> FindByNameAsync(string countryName)
        {
            await using var conn = _executor.CreateConnection();
            using var cmd = _executor.CreateCommand(conn, "usp_Countries_GetByName");

            // Input Parameters
            cmd.Parameters.Add("@CountryName", SqlDbType.NVarChar, 50).Value = countryName;
            return await _executor.ExecuteSingleAsync(cmd, conn, MapToCountry);
        }

        public async Task<OperationResult<bool>> ExistsAsync(int countryId)
        {
            await using var conn = _executor.CreateConnection();
            using var cmd = _executor.CreateCommand(conn, "usp_Countries_ExistsById");

            cmd.Parameters.Add("@CountryId", SqlDbType.Int).Value = countryId;
            _executor.AttachStatusParameters(cmd, out SqlParameter code, out SqlParameter message);

            await conn.OpenAsync();
            var result = await cmd.ExecuteScalarAsync() != null;

            return _executor.CreateOperationResult(result, code, message);
        }

        public async Task<OperationResult<bool>> ExistsAsync(string countryName)
        {
            await using var conn = _executor.CreateConnection();
            using var cmd = _executor.CreateCommand(conn, "usp_Countries_ExistsByName");

            cmd.Parameters.Add("@CountryName", SqlDbType.NVarChar, 50).Value = countryName;
            _executor.AttachStatusParameters(cmd, out SqlParameter code, out SqlParameter message);

            await conn.OpenAsync();
            var result = await cmd.ExecuteScalarAsync() != null;

            return _executor.CreateOperationResult(result, code, message);
        }

        public async Task<OperationResult<IReadOnlyList<Country>>> GetAllAsync()
        {
            await using var conn = _executor.CreateConnection();
            using var cmd = _executor.CreateCommand(conn, "usp_Countries_GetAll");

            return await _executor.ExecuteListAsync(cmd, conn, MapToCountry);
        }

        public async Task<OperationResult<PaginationResponse<Country>>> GetPagedAsync(PaginationRequest request)
        {
            await using var conn = _executor.CreateConnection();
            using var cmd = _executor.CreateCommand(conn, "usp_Countries_GetPaged");

            return await _executor.ExecutePaginationAsync(cmd, conn, request, MapToCountry);
        }

        public async Task<OperationResult<bool>> UpdateAsync(Country country)
        {
            await using var conn = _executor.CreateConnection();
            using var cmd = _executor.CreateCommand(conn, "usp_Countries_Update");

            AddCountryParameters(cmd, country, addCountryId: true);
            return await _executor.ExecuteNonQueryAsync(cmd, conn);
        }

        public async Task<OperationResult<bool>> DeleteAsync(int countryId)
        {
            await using var conn = _executor.CreateConnection();
            using var cmd = _executor.CreateCommand(conn, "usp_Countries_Delete");

            cmd.Parameters.Add("@CountryId", SqlDbType.Int).Value = countryId;
            return await _executor.ExecuteNonQueryAsync(cmd, conn);
        }


        private static Country MapToCountry(SqlDataReader reader)
        {
            return new Country
            (
                countryId: reader.GetInt32(reader.GetOrdinal("CountryId")),
                countryName: reader.GetString(reader.GetOrdinal("CountryName"))
            );
        }

        private static void AddCountryParameters(SqlCommand cmd, Country country, bool addCountryId = false)
        {
            if (addCountryId)
            {
                cmd.Parameters.Add("@CountryId", SqlDbType.Int).Value = country.CountryId;
            }
            cmd.Parameters.Add("@CountryName", SqlDbType.NVarChar, 50).Value = country.CountryName;
        }
    }
}

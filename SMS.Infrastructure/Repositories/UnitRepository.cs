using Microsoft.Data.SqlClient;
using SMS.Application.Common.Results;
using SMS.Application.Interfaces.DataAccess;
using SMS.Application.Interfaces.Repositories;
using SMS.Domain.Entities;
using SMS.Shared.Common;
using System.Data;

namespace SMS.Infrastructure.Repositories
{
    public class UnitRepository : IUnitRepository
    {
        private readonly IStoredProcedureExecutor _executor;

        public UnitRepository(IStoredProcedureExecutor executor)
        {
            _executor = executor;
        }

        public async Task<OperationResult<int>> AddAsync(Unit unit)
        {
            await using var conn = _executor.CreateConnection();
            await using var cmd = _executor.CreateCommand(conn, "usp_Units_Insert");

            AddParameters(cmd, unit);

            var insertedIdParam = new SqlParameter("@InsertedId", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(insertedIdParam);

            return await _executor.ExecuteNonQueryAsync<int>(cmd, conn, insertedIdParam);
        }

        public async Task<OperationResult<Unit?>> FindByIdAsync(int unitId)
        {
            await using var conn = _executor.CreateConnection();
            await using var cmd = _executor.CreateCommand(conn, "usp_Units_GetById");

            cmd.Parameters.Add("@UnitId", SqlDbType.Int).Value = unitId;
            return await _executor.ExecuteSingleAsync(cmd, conn, MapToUnit);
        }

        public async Task<OperationResult<Unit?>> FindByNameAsync(string unitName)
        {
            await using var conn = _executor.CreateConnection();
            await using var cmd = _executor.CreateCommand(conn, "usp_Units_GetByName");

            cmd.Parameters.Add("@UnitName", SqlDbType.NVarChar, 20).Value = unitName;
            return await _executor.ExecuteSingleAsync(cmd, conn, MapToUnit);
        }

        public async Task<OperationResult<Unit?>> FindBySymbolAsync(string symbol)
        {
            await using var conn = _executor.CreateConnection();
            await using var cmd = _executor.CreateCommand(conn, "usp_Units_GetBySymbol");

            cmd.Parameters.Add("@Symbol", SqlDbType.NVarChar, 10).Value = symbol;
            return await _executor.ExecuteSingleAsync(cmd, conn, MapToUnit);
        }

        public async Task<OperationResult<PaginationResponse<Unit>>> GetPagedByIsDecimalAsync(PaginationRequest request,
            bool isDecimal)
        {
            await using var conn = _executor.CreateConnection();
            await using var cmd = _executor.CreateCommand(conn, "usp_Units_GetPagedByIsDecimal");

            cmd.Parameters.Add("@IsDecimal", SqlDbType.Bit).Value = isDecimal;
            return await _executor.ExecutePaginationAsync(cmd, conn, request, MapToUnit);
        }

        public async Task<OperationResult<bool>> UpdateAsync(Unit unit)
        {
            await using var conn = _executor.CreateConnection();
            await using var cmd = _executor.CreateCommand(conn, "usp_Units_Update");

            AddParameters(cmd, unit, isUpdate: true);
            return await _executor.ExecuteBooleanOperationAsync(cmd, conn);
        }

        private static Unit MapToUnit(SqlDataReader reader)
        {
            return new Unit(
                unitId: reader.GetInt32(reader.GetOrdinal("UnitId")),
                unitName: reader.GetString(reader.GetOrdinal("UnitName")),
                symbol: reader.GetString(reader.GetOrdinal("Symbol")),
                isDecimal: reader.GetBoolean(reader.GetOrdinal("IsDecimal")));
        }

        private static void AddParameters(SqlCommand cmd, Unit unit, bool isUpdate = false)
        {
            cmd.Parameters.Add("@UnitName", SqlDbType.NVarChar, 20).Value = unit.UnitName;
            cmd.Parameters.Add("@Symbol", SqlDbType.NVarChar, 10).Value = unit.Symbol;
            cmd.Parameters.Add("@IsDecimal", SqlDbType.Bit).Value = unit.IsDecimal;

            if (isUpdate)
            {
                cmd.Parameters.Add("@UnitId", SqlDbType.Int).Value = unit.UnitId;
            }
        }
    }
}
using Microsoft.Data.SqlClient;
using SMS.Application.Common.Results;
using SMS.Application.Interfaces.DataAccess;
using SMS.Application.Interfaces.Repositories;
using SMS.Domain.Entities;
using SMS.Shared.Common;
using System.Data;

namespace SMS.Infrastructure.Repositories
{
    public class ReturnRepository : IReturnRepository
    {
        private readonly IStoredProcedureExecutor _executor;

        public ReturnRepository(IStoredProcedureExecutor executor)
        {
            _executor = executor;
        }

        public async Task<OperationResult<int>> AddAsync(Return returnEntity)
        {
            await using var conn = _executor.CreateConnection();
            await using var cmd = _executor.CreateCommand(conn, "usp_Returns_Insert");

            cmd.Parameters.Add("@SaleId", SqlDbType.Int).Value = returnEntity.SaleId;
            cmd.Parameters.Add("@ReturnReason", SqlDbType.NVarChar, 250).Value = returnEntity.ReturnReason ?? (object)DBNull.Value;
            cmd.Parameters.Add("@ReturnTotal", SqlDbType.Decimal).Value = returnEntity.ReturnTotal;
            cmd.Parameters.Add("@CreatedBy", SqlDbType.Int).Value = returnEntity.CreatedBy;

            var insertedIdParam = new SqlParameter("@InsertedId", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(insertedIdParam);

            return await _executor.ExecuteNonQueryAsync<int>(cmd, conn, insertedIdParam);
        }

        public async Task<OperationResult<Return?>> FindByIdAsync(int returnId)
        {
            await using var conn = _executor.CreateConnection();
            await using var cmd = _executor.CreateCommand(conn, "usp_Returns_GetById");

            cmd.Parameters.Add("@ReturnId", SqlDbType.Int).Value = returnId;
            return await _executor.ExecuteSingleAsync(cmd, conn, MapToReturn);
        }

        public async Task<OperationResult<PaginationResponse<Return>>> GetPagedAsync(PaginationRequest paginationRequest)
        {
            await using var conn = _executor.CreateConnection();
            await using var cmd = _executor.CreateCommand(conn, "usp_Returns_GetPaged");

            return await _executor.ExecutePaginationAsync(cmd, conn, paginationRequest, MapToReturn);
        }

        public async Task<OperationResult<IReadOnlyList<Return>>> GetBySaleIdAsync(int saleId)
        {
            await using var conn = _executor.CreateConnection();
            await using var cmd = _executor.CreateCommand(conn, "usp_Returns_GetBySaleId");

            cmd.Parameters.Add("@SaleId", SqlDbType.Int).Value = saleId;
            return await _executor.ExecuteListAsync(cmd, conn, MapToReturn);
        }

        public async Task<OperationResult<PaginationResponse<Return>>> GetPagedByCustomerIdAsync(int customerId, PaginationRequest paginationRequest)
        {
            await using var conn = _executor.CreateConnection();
            await using var cmd = _executor.CreateCommand(conn, "usp_Returns_GetPagedByCustomerId");

            cmd.Parameters.Add("@CustomerId", SqlDbType.Int).Value = customerId;
            return await _executor.ExecutePaginationAsync(cmd, conn, paginationRequest, MapToReturn);
        }

        public async Task<OperationResult<PaginationResponse<Return>>> GetPagedByDateRangeAsync(DateTime? startDate, DateTime? endDate, PaginationRequest paginationRequest)
        {
            await using var conn = _executor.CreateConnection();
            await using var cmd = _executor.CreateCommand(conn, "usp_Returns_GetPagedByDateRange");

            cmd.Parameters.Add("@StartDate", SqlDbType.DateTime2).Value = startDate ?? (object)DBNull.Value;
            cmd.Parameters.Add("@EndDate", SqlDbType.DateTime2).Value = endDate ?? (object)DBNull.Value;
            return await _executor.ExecutePaginationAsync(cmd, conn, paginationRequest, MapToReturn);
        }

        public async Task<OperationResult<PaginationResponse<Return>>> GetPagedByReturnTotalRangeAsync(decimal minTotal, decimal maxTotal, PaginationRequest paginationRequest)
        {
            await using var conn = _executor.CreateConnection();
            await using var cmd = _executor.CreateCommand(conn, "usp_Returns_GetPagedByReturnTotalRange");

            cmd.Parameters.Add("@MinReturnTotal", SqlDbType.Decimal).Value = minTotal;
            cmd.Parameters.Add("@MaxReturnTotal", SqlDbType.Decimal).Value = maxTotal;

            return await _executor.ExecutePaginationAsync(cmd, conn, paginationRequest, MapToReturn);
        }

        public async Task<OperationResult<decimal>> GetReturnTotalByIdAsync(int returnId)
        {
            await using var conn = _executor.CreateConnection();
            await using var cmd = _executor.CreateCommand(conn, "usp_Returns_GetReturnTotal");

            cmd.Parameters.Add("@ReturnId", SqlDbType.Int).Value = returnId;
            return await _executor.ExecuteScalarAsync<decimal>(cmd, conn);
        }

        private static Return MapToReturn(SqlDataReader reader)
        {
            var customerIdOrdinal = reader.GetOrdinal("CustomerId");
            int? customerId = reader.IsDBNull(customerIdOrdinal) ? null : reader.GetInt32(customerIdOrdinal);

            var returnReasonOrdinal = reader.GetOrdinal("ReturnReason");
            string? returnReason = reader.IsDBNull(returnReasonOrdinal) ? null : reader.GetString(returnReasonOrdinal);

            return new Return(
                returnId: reader.GetInt32(reader.GetOrdinal("ReturnId")),
                saleId: reader.GetInt32(reader.GetOrdinal("SaleId")),
                customerId: customerId,
                returnReason: returnReason,
                returnTotal: reader.GetDecimal(reader.GetOrdinal("ReturnTotal")),
                createdBy: reader.GetInt32(reader.GetOrdinal("CreatedBy")),
                returnDate: reader.GetDateTime(reader.GetOrdinal("ReturnDate")));
        }
    }
}

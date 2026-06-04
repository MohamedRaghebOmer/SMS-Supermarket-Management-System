using Microsoft.Data.SqlClient;
using SMS.Application.Common.Results;
using SMS.Application.Interfaces.DataAccess;
using SMS.Application.Interfaces.Repositories;
using SMS.Domain.Entities;
using SMS.Shared.Common;
using System.Data;

namespace SMS.Infrastructure.Repositories
{
    public class SaleRepository : ISaleRepository
    {
        private readonly IStoredProcedureExecutor _executor;

        public SaleRepository(IStoredProcedureExecutor executor)
        {
            _executor = executor;
        }

        public async Task<OperationResult<int>> AddAsync(Sale sale)
        {
            await using var conn = _executor.CreateConnection();
            using var cmd = _executor.CreateCommand(conn, "usp_Sales_Insert");

            AddSaleParameters(cmd, sale);

            var insertedIdParam = new SqlParameter("@InsertedId", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(insertedIdParam);

            return await _executor.ExecuteNonQueryAsync<int>(cmd, conn, insertedIdParam);
        }

        public async Task<OperationResult<Sale?>> FindByIdAsync(int saleId)
        {
            await using var conn = _executor.CreateConnection();
            using var cmd = _executor.CreateCommand(conn, "usp_Sales_GetById");

            cmd.Parameters.Add("@SaleId", SqlDbType.Int).Value = saleId;
            return await _executor.ExecuteSingleAsync(cmd, conn, MapToSale);
        }

        public async Task<OperationResult<PaginationResponse<Sale>>> GetPagedAsync(
            PaginationRequest paginationRequest)
        {
            await using var conn = _executor.CreateConnection();
            using var cmd = _executor.CreateCommand(conn, "usp_Sales_GetPaged");

            return await _executor.ExecutePaginationAsync(cmd, conn, paginationRequest, MapToSale);
        }

        public async Task<OperationResult<PaginationResponse<Sale>>> GetPagedByCashierIdAsync(
            int cashierId, PaginationRequest paginationRequest)
        {
            await using var conn = _executor.CreateConnection();
            using var cmd = _executor.CreateCommand(conn, "usp_Sales_GetPagedByCashierId");

            cmd.Parameters.Add("@CashierId", SqlDbType.Int).Value = cashierId;
            return await _executor.ExecutePaginationAsync(cmd, conn, paginationRequest, MapToSale);
        }

        public async Task<OperationResult<PaginationResponse<Sale>>> GetPagedByCustomerIdAsync(
            int customerId, PaginationRequest paginationRequest)
        {
            await using var conn = _executor.CreateConnection();
            using var cmd = _executor.CreateCommand(conn, "usp_Sales_GetPagedByCustomerId");

            cmd.Parameters.Add("@CustomerId", SqlDbType.Int).Value = customerId;
            return await _executor.ExecutePaginationAsync(cmd, conn, paginationRequest, MapToSale);
        }

        public async Task<OperationResult<PaginationResponse<Sale>>> GetPagedByDateRangeAsync(
            DateTime startDate, DateTime endDate, PaginationRequest paginationRequest)
        {
            await using var conn = _executor.CreateConnection();
            using var cmd = _executor.CreateCommand(conn, "usp_Sales_GetPagedByDateRange");

            cmd.Parameters.Add("@StartDate", SqlDbType.DateTime2).Value = startDate;
            cmd.Parameters.Add("@EndDate", SqlDbType.DateTime2).Value = endDate;
            return await _executor.ExecutePaginationAsync(cmd, conn, paginationRequest, MapToSale);
        }

        public async Task<OperationResult<bool>> ExistsByIdAsync(int saleId)
        {
            await using var conn = _executor.CreateConnection();
            using var cmd = _executor.CreateCommand(conn, "usp_Sales_ExistsById");

            cmd.Parameters.Add("@SaleId", SqlDbType.Int).Value = saleId;
            return await _executor.ExecuteExistsAsync(conn, cmd);
        }


        private static Sale MapToSale(SqlDataReader reader)
        {
            var customerIdOrdinal = reader.GetOrdinal("CustomerId");
            int? customerId = reader.IsDBNull(customerIdOrdinal)
                ? null
                : reader.GetInt32(customerIdOrdinal);

            var paymentMethodOrdinal = reader.GetOrdinal("PaymentMethod");
            SMS.Shared.Enums.PaymentMethod? paymentMethod = reader.IsDBNull(paymentMethodOrdinal)
                ? null
                : (SMS.Shared.Enums.PaymentMethod)reader.GetByte(paymentMethodOrdinal);

            var notesOrdinal = reader.GetOrdinal("Notes");
            string? notes = reader.IsDBNull(notesOrdinal) ? null : reader.GetString(notesOrdinal);

            return new Sale(
                saleId: reader.GetInt32(reader.GetOrdinal("SaleId")),
                saleDate: reader.GetDateTime(reader.GetOrdinal("SaleDate")),
                customerId: customerId,
                cashierId: reader.GetInt32(reader.GetOrdinal("CashierId")),
                paymentMethod: paymentMethod,
                subTotal: reader.GetDecimal(reader.GetOrdinal("SubTotal")),
                discountAmount: reader.GetDecimal(reader.GetOrdinal("DiscountAmount")),
                netTotal: reader.GetDecimal(reader.GetOrdinal("NetTotal")),
                paidAmount: reader.GetDecimal(reader.GetOrdinal("PaidAmount")),
                changeAmount: reader.GetDecimal(reader.GetOrdinal("ChangeAmount")),
                isCredit: reader.GetBoolean(reader.GetOrdinal("IsCredit")),
                notes: notes);
        }

        private static void AddSaleParameters(SqlCommand cmd, Sale sale)
        {
            cmd.Parameters.Add("@CustomerId", SqlDbType.Int).Value = sale.CustomerId ?? (object)DBNull.Value;
            cmd.Parameters.Add("@CashierId", SqlDbType.Int).Value = sale.CashierId;
            cmd.Parameters.Add("@PaymentMethod", SqlDbType.TinyInt).Value = sale.PaymentMethod.HasValue
                ? (byte)sale.PaymentMethod.Value
                : (object)DBNull.Value;
            cmd.Parameters.Add("@SubTotal", SqlDbType.Decimal).Value = sale.SubTotal;
            cmd.Parameters.Add("@DiscountAmount", SqlDbType.Decimal).Value = sale.DiscountAmount;
            cmd.Parameters.Add("@NetTotal", SqlDbType.Decimal).Value = sale.NetTotal;
            cmd.Parameters.Add("@PaidAmount", SqlDbType.Decimal).Value = sale.PaidAmount;
            cmd.Parameters.Add("@ChangeAmount", SqlDbType.Decimal).Value = sale.ChangeAmount;
            cmd.Parameters.Add("@IsCredit", SqlDbType.Bit).Value = sale.IsCredit;
            cmd.Parameters.Add("@Notes", SqlDbType.NVarChar, 250).Value = sale.Notes ?? (object)DBNull.Value;
        }
    }
}
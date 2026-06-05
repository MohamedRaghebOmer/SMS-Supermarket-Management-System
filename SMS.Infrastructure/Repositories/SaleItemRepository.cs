using Microsoft.Data.SqlClient;
using SMS.Application.Common.Results;
using SMS.Application.Interfaces.DataAccess;
using SMS.Application.Interfaces.Repositories;
using SMS.Domain.Entities;
using SMS.Shared.Common;
using System.Data;

namespace SMS.Infrastructure.Repositories
{
    public class SaleItemRepository : ISaleItemRepository
    {
        private readonly IStoredProcedureExecutor _executor;

        public SaleItemRepository(IStoredProcedureExecutor executor)
        {
            _executor = executor;
        }

        public async Task<OperationResult<int>> AddAsync(SaleItem saleItem)
        {
            await using var conn = _executor.CreateConnection();
            using var cmd = _executor.CreateCommand(conn, "usp_SaleItems_Insert");

            AddSaleItemParameters(cmd, saleItem);

            var insertedIdParam = new SqlParameter("@InsertedId", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(insertedIdParam);

            return await _executor.ExecuteNonQueryAsync<int>(cmd, conn, insertedIdParam);
        }

        public async Task<OperationResult<SaleItem?>> FindByIdAsync(int saleItemId)
        {
            await using var conn = _executor.CreateConnection();
            using var cmd = _executor.CreateCommand(conn, "usp_SaleItems_GetById");

            cmd.Parameters.Add("@SaleItemId", SqlDbType.Int).Value = saleItemId;
            return await _executor.ExecuteSingleAsync(cmd, conn, MapToSaleItem);
        }

        public async Task<OperationResult<PaginationResponse<SaleItem>>> GetPagedBySaleIdAsync(int saleId, PaginationRequest request)
        {
            await using var conn = _executor.CreateConnection();
            using var cmd = _executor.CreateCommand(conn, "usp_SaleItems_GetPagedBySaleId");

            cmd.Parameters.Add("@SaleId", SqlDbType.Int).Value = saleId;
            return await _executor.ExecutePaginationAsync(cmd, conn, request, MapToSaleItem);
        }

        public async Task<OperationResult<PaginationResponse<SaleItem>>> GetPagedByProductIdAsync(int productId, PaginationRequest request)
        {
            await using var conn = _executor.CreateConnection();
            using var cmd = _executor.CreateCommand(conn, "usp_SaleItems_GetPagedByProductId");

            cmd.Parameters.Add("@ProductId", SqlDbType.Int).Value = productId;
            return await _executor.ExecutePaginationAsync(cmd, conn, request, MapToSaleItem);
        }

        public async Task<OperationResult<PaginationResponse<SaleItem>>> GetPagedAsync(PaginationRequest request)
        {
            await using var conn = _executor.CreateConnection();
            using var cmd = _executor.CreateCommand(conn, "usp_SaleItems_GetPaged");

            return await _executor.ExecutePaginationAsync(cmd, conn, request, MapToSaleItem);
        }

        public async Task<OperationResult<SaleItem?>> FindBySaleIdAndProductIdAsync(int saleId, int productId)
        {
            await using var conn = _executor.CreateConnection();
            using var cmd = _executor.CreateCommand(conn, "usp_SaleItems_GetBySaleIdAndProductId");

            cmd.Parameters.Add("@SaleId", SqlDbType.Int).Value = saleId;
            cmd.Parameters.Add("@ProductId", SqlDbType.Int).Value = productId;

            return await _executor.ExecuteSingleAsync(cmd, conn, MapToSaleItem);
        }

        public async Task<OperationResult<decimal>> GetLineTotalByIdAsync(int saleItemId)
        {
            await using var conn = _executor.CreateConnection();
            await using var cmd = _executor.CreateCommand(conn, "usp_SaleItems_GetLineTotalById");

            cmd.Parameters.Add("@SaleItemId", SqlDbType.Int).Value = saleItemId;
            return await _executor.ExecuteScalarAsync<Decimal>(cmd, conn);
        }

        public async Task<OperationResult<bool>> UpdateAsync(SaleItem saleItem)
        {
            await using var conn = _executor.CreateConnection();
            using var cmd = _executor.CreateCommand(conn, "usp_SaleItems_Update");

            AddSaleItemParameters(cmd, saleItem, true);

            return await _executor.ExecuteBooleanOperationAsync(cmd, conn);
        }

        public void AddSaleItemParameters(SqlCommand cmd, SaleItem saleItem,
            bool isUpdate = false)
        {
            if (isUpdate)
            {
                cmd.Parameters.Add("@SaleItemId", SqlDbType.Int).Value = saleItem.SaleItemId;
            }

            cmd.Parameters.Add("@SaleId", SqlDbType.Int).Value = saleItem.SaleId;
            cmd.Parameters.Add("@ProductId", SqlDbType.Int).Value = saleItem.ProductId;
            cmd.Parameters.Add("@Quantity", SqlDbType.Decimal).Value = saleItem.Quantity;
            cmd.Parameters.Add("@UnitSellPriceAtSale", SqlDbType.Decimal).Value = saleItem.UnitSellPriceAtSale;
            cmd.Parameters.Add("@DiscountAmount", SqlDbType.Decimal).Value = saleItem.DiscountAmount;
            cmd.Parameters.Add("@LineTotal", SqlDbType.Decimal).Value = saleItem.LineTotal;
        }

        public async Task<OperationResult<bool>> DeleteAsync(int saleItemId)
        {
            await using var conn = _executor.CreateConnection();
            using var cmd = _executor.CreateCommand(conn, "usp_SaleItems_Delete");

            cmd.Parameters.Add("@SaleItemId", SqlDbType.Int).Value = saleItemId;
            return await _executor.ExecuteBooleanOperationAsync(cmd, conn);
        }

        private static SaleItem MapToSaleItem(SqlDataReader reader)
        {
            return new SaleItem(
                saleItemId: reader.GetInt32(reader.GetOrdinal("SaleItemId")),
                saleId: reader.GetInt32(reader.GetOrdinal("SaleId")),
                productId: reader.GetInt32(reader.GetOrdinal("ProductId")),
                quantity: reader.GetDecimal(reader.GetOrdinal("Quantity")),
                unitCostPriceAtSale: reader.GetDecimal(reader.GetOrdinal("UnitCostPriceAtSale")),
                unitSellPriceAtSale: reader.GetDecimal(reader.GetOrdinal("UnitSellPriceAtSale")),
                discountAmount: reader.GetDecimal(reader.GetOrdinal("DiscountAmount")),
                lineTotal: reader.GetDecimal(reader.GetOrdinal("LineTotal"))
            );
        }
    }
}

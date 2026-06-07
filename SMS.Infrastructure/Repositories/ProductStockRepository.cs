using Microsoft.Data.SqlClient;
using SMS.Application.Common.Results;
using SMS.Application.Interfaces.DataAccess;
using SMS.Application.Interfaces.Repositories;
using SMS.Domain.Entities;
using SMS.Shared.Common;
using System.Data;

namespace SMS.Infrastructure.Repositories
{
    public class ProductStockRepository : IProductStockRepository
    {
        private readonly IStoredProcedureExecutor _executor;

        public ProductStockRepository(IStoredProcedureExecutor executor)
        {
            _executor = executor;
        }

        public async Task<OperationResult<PaginationResponse<ProductStock>>> GetPagedAsync(PaginationRequest request)
        {
            await using var conn = _executor.CreateConnection();
            using var cmd = _executor.CreateCommand(conn, "usp_ProductStock_GetPaged");

            return await _executor.ExecutePaginationAsync(cmd, conn, request, MapToProductStock);
        }

        public async Task<OperationResult<ProductStock?>> FindByIdAsync(int productId)
        {
            await using var conn = _executor.CreateConnection();
            using var cmd = _executor.CreateCommand(conn, "usp_ProductStock_GetById");

            cmd.Parameters.Add("@ProductId", SqlDbType.Int).Value = productId;
            return await _executor.ExecuteSingleAsync(cmd, conn, MapToProductStock);
        }

        public async Task<OperationResult<decimal>> GetQuantityOnHandAsync(int productId)
        {
            await using var conn = _executor.CreateConnection();
            using var cmd = _executor.CreateCommand(conn, "usp_ProductStock_GetQuantityOnHand");

            cmd.Parameters.Add("@ProductId", SqlDbType.Int).Value = productId;
            return await _executor.ExecuteScalarAsync<decimal>(cmd, conn);
        }

        public async Task<OperationResult<decimal>> GetReorderLevelAsync(int productId)
        {
            await using var conn = _executor.CreateConnection();
            using var cmd = _executor.CreateCommand(conn, "usp_ProductStock_GetReorderLevel");

            cmd.Parameters.Add("@ProductId", SqlDbType.Int).Value = productId;
            return await _executor.ExecuteScalarAsync<decimal>(cmd, conn);
        }

        public async Task<OperationResult<bool>> UpdateReorderLevelAsync(int productId, decimal reorderLevel)
        {
            await using var conn = _executor.CreateConnection();
            await using var cmd = _executor.CreateCommand(conn, "usp_ProductStock_UpdateReorderLevel");

            cmd.Parameters.Add("@ProductId", SqlDbType.Int).Value = productId;
            cmd.Parameters.Add("@ReorderLevel", SqlDbType.Decimal).Value = reorderLevel;
            return await _executor.ExecuteBooleanOperationAsync(cmd, conn);
        }

        private static ProductStock MapToProductStock(SqlDataReader reader)
        {
            return new ProductStock(
                productId: reader.GetInt32(reader.GetOrdinal("ProductId")),
                quantityOnHand: reader.GetDecimal(reader.GetOrdinal("QuantityOnHand")),
                reorderLevel: reader.GetDecimal(reader.GetOrdinal("ReorderLevel")),
                updatedAt: reader.GetDateTime(reader.GetOrdinal("UpdatedAt")));
        }
    }
}

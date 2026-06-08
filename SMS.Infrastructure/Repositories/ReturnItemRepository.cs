using Microsoft.Data.SqlClient;
using SMS.Application.Common.Results;
using SMS.Application.Interfaces.DataAccess;
using SMS.Application.Interfaces.Repositories;
using SMS.Domain.Entities;
using SMS.Shared.Common;
using System.Data;

namespace SMS.Infrastructure.Repositories
{
    public class ReturnItemRepository : IReturnItemRepository
    {
        private readonly IStoredProcedureExecutor _executor;

        public ReturnItemRepository(IStoredProcedureExecutor executor)
        {
            _executor = executor;
        }

        public async Task<OperationResult<int>> AddAsync(ReturnItem returnItem)
        {
            await using var conn = _executor.CreateConnection();
            await using var cmd = _executor.CreateCommand(conn, "usp_ReturnItems_Insert");

            AddParameters(cmd, returnItem);

            var insertedIdParam = new SqlParameter("@InsertedId", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(insertedIdParam);

            return await _executor.ExecuteNonQueryAsync<int>(cmd, conn, insertedIdParam);
        }

        public async Task<OperationResult<ReturnItem?>> FindByIdAsync(int returnItemId)
        {
            await using var conn = _executor.CreateConnection();
            await using var cmd = _executor.CreateCommand(conn, "usp_ReturnItems_GetById");

            cmd.Parameters.Add("@ReturnItemId", SqlDbType.Int).Value = returnItemId;
            return await _executor.ExecuteSingleAsync(cmd, conn, MapToReturnItem);
        }

        public async Task<OperationResult<PaginationResponse<ReturnItem>>> GetPagedAsync(PaginationRequest request)
        {
            await using var conn = _executor.CreateConnection();
            await using var cmd = _executor.CreateCommand(conn, "usp_ReturnItems_GetPaged");

            return await _executor.ExecutePaginationAsync(cmd, conn, request, MapToReturnItem);
        }

        public async Task<OperationResult<PaginationResponse<ReturnItem>>> GetPagedByReturnIdAsync(int returnId, PaginationRequest request)
        {
            await using var conn = _executor.CreateConnection();
            await using var cmd = _executor.CreateCommand(conn, "usp_ReturnItems_GetPagedByReturnId");

            cmd.Parameters.Add("@ReturnId", SqlDbType.Int).Value = returnId;
            return await _executor.ExecutePaginationAsync(cmd, conn, request, MapToReturnItem);
        }

        public async Task<OperationResult<PaginationResponse<ReturnItem>>> GetPagedByProductIdAsync(int productId, PaginationRequest request)
        {
            await using var conn = _executor.CreateConnection();
            await using var cmd = _executor.CreateCommand(conn, "usp_ReturnItems_GetPagedByProductId");

            cmd.Parameters.Add("@ProductId", SqlDbType.Int).Value = productId;
            return await _executor.ExecutePaginationAsync(cmd, conn, request, MapToReturnItem);
        }

        private static ReturnItem MapToReturnItem(SqlDataReader reader)
        {
            return new ReturnItem(
                returnItemId: reader.GetInt32(reader.GetOrdinal("ReturnItemId")),
                returnId: reader.GetInt32(reader.GetOrdinal("ReturnId")),
                saleItemId: reader.GetInt32(reader.GetOrdinal("SaleItemId")),
                productId: reader.GetInt32(reader.GetOrdinal("ProductId")),
                quantity: reader.GetDecimal(reader.GetOrdinal("Quantity")),
                unitPrice: reader.GetDecimal(reader.GetOrdinal("UnitPrice")),
                lineTotal: reader.GetDecimal(reader.GetOrdinal("LineTotal"))
            );
        }

        private static void AddParameters(SqlCommand cmd, ReturnItem returnItem)
        {
            cmd.Parameters.Add("@ReturnId", SqlDbType.Int).Value = returnItem.ReturnId;
            cmd.Parameters.Add("@SaleItemId", SqlDbType.Int).Value = returnItem.SaleItemId;
            cmd.Parameters.Add("@Quantity", SqlDbType.Decimal).Value = returnItem.Quantity;
            cmd.Parameters.Add("@UnitPrice", SqlDbType.Decimal).Value = returnItem.UnitPrice;
            cmd.Parameters.Add("@LineTotal", SqlDbType.Decimal).Value = returnItem.LineTotal;
        }
    }
}

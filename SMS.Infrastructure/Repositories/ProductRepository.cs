using Microsoft.Data.SqlClient;
using SMS.Application.Common.Results;
using SMS.Application.Interfaces.DataAccess;
using SMS.Application.Interfaces.Repositories;
using SMS.Domain.Entities;
using SMS.Shared.Common;
using System.Data;

namespace SMS.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly IStoredProcedureExecutor _executor;

        public ProductRepository(IStoredProcedureExecutor executor)
        {
            _executor = executor;
        }

        public async Task<OperationResult<int>> AddAsync(Product product)
        {
            await using var conn = _executor.CreateConnection();
            using var cmd = _executor.CreateCommand(conn, "usp_Products_Insert");

            AddParameters(cmd, product, isUpdate: false);

            var insertedIdParam = new SqlParameter("@InsertedId", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(insertedIdParam);

            return await _executor.ExecuteNonQueryAsync<int>(cmd, conn, insertedIdParam);
        }

        public async Task<OperationResult<Product?>> FindByIdAsync(int productId)
        {
            await using var conn = _executor.CreateConnection();
            using var cmd = _executor.CreateCommand(conn, "usp_Products_GetById");

            cmd.Parameters.Add("@ProductId", SqlDbType.Int).Value = productId;
            return await _executor.ExecuteSingleAsync(cmd, conn, MapToProduct);
        }

        public async Task<OperationResult<PaginationResponse<Product>>> GetPagedByCategoryIdAsync(int categoryId,
            PaginationRequest request)
        {
            await using var conn = _executor.CreateConnection();
            using var cmd = _executor.CreateCommand(conn, "usp_Products_GetPagedByCategoryId");

            cmd.Parameters.Add("@CategoryId", SqlDbType.Int).Value = categoryId;
            return await _executor.ExecutePaginationAsync(cmd, conn, request, MapToProduct);
        }

        public async Task<OperationResult<PaginationResponse<Product>>> GetPagedAsync(PaginationRequest request)
        {
            await using var conn = _executor.CreateConnection();
            using var cmd = _executor.CreateCommand(conn, "usp_Products_GetPaged");

            return await _executor.ExecutePaginationAsync(cmd, conn, request, MapToProduct);
        }

        public async Task<OperationResult<Product?>> FindByNameAsync(string productName)
        {
            await using var conn = _executor.CreateConnection();
            using var cmd = _executor.CreateCommand(conn, "usp_Products_GetByName");

            cmd.Parameters.Add("@ProductName", SqlDbType.NVarChar, 150).Value = productName;
            return await _executor.ExecuteSingleAsync(cmd, conn, MapToProduct);
        }

        public async Task<OperationResult<Product?>> FindBySkuAsync(string sku)
        {
            await using var conn = _executor.CreateConnection();
            using var cmd = _executor.CreateCommand(conn, "usp_Products_GetBySku");

            cmd.Parameters.Add("@SKU", SqlDbType.NVarChar, 50).Value = sku;
            return await _executor.ExecuteSingleAsync(cmd, conn, MapToProduct);
        }

        public async Task<OperationResult<PaginationResponse<Product>>> GetPagedByUnitIdAsync(int unitId,
            PaginationRequest request)
        {
            await using var conn = _executor.CreateConnection();
            using var cmd = _executor.CreateCommand(conn, "usp_Products_GetPagedByUnitId");

            cmd.Parameters.Add("@UnitId", SqlDbType.Int).Value = unitId;
            return await _executor.ExecutePaginationAsync(cmd, conn, request, MapToProduct);
        }

        public async Task<OperationResult<PaginationResponse<Product>>> GetPagedByDiscountRangeAsync(
            PaginationRequest request, decimal minPercent, decimal maxPercent)
        {
            await using var conn = _executor.CreateConnection();
            using var cmd = _executor.CreateCommand(conn, "usp_Products_GetPagedByDiscountPercentRange");

            cmd.Parameters.Add("@MinDiscountPercent", SqlDbType.Decimal).Value = minPercent;
            cmd.Parameters.Add("@MaxDiscountPercent", SqlDbType.Decimal).Value = maxPercent;
            return await _executor.ExecutePaginationAsync(cmd, conn, request, MapToProduct);
        }

        public async Task<OperationResult<PaginationResponse<Product>>> GetPagedByIsActiveAsync(
            PaginationRequest request, bool isActive)
        {
            await using var conn = _executor.CreateConnection();
            using var cmd = _executor.CreateCommand(conn, "usp_Products_GetPagedByIsActive");

            cmd.Parameters.Add("@IsActive", SqlDbType.Bit).Value = isActive;
            return await _executor.ExecutePaginationAsync(cmd, conn, request, MapToProduct);
        }

        public async Task<OperationResult<PaginationResponse<Product>>> GetPagedByCreatedAtRangeAsync(
            PaginationRequest request, DateTime from, DateTime to)
        {
            await using var conn = _executor.CreateConnection();
            using var cmd = _executor.CreateCommand(conn, "usp_Products_GetPagedByCreatedAtRange");

            cmd.Parameters.Add("@MinCreatedAt", SqlDbType.DateTime2).Value = from;
            cmd.Parameters.Add("@MaxCreatedAt", SqlDbType.DateTime2).Value = to;
            return await _executor.ExecutePaginationAsync(cmd, conn, request, MapToProduct);
        }

        public async Task<OperationResult<PaginationResponse<Product>>> GetPagedByUpdatedAtRangeAsync(
            PaginationRequest request, DateTime from, DateTime to)
        {
            await using var conn = _executor.CreateConnection();
            using var cmd = _executor.CreateCommand(conn, "usp_Products_GetPagedByUpdatedAtRange");

            cmd.Parameters.Add("@MinUpdatedAt", SqlDbType.DateTime2).Value = from;
            cmd.Parameters.Add("@MaxUpdatedAt", SqlDbType.DateTime2).Value = to;
            return await _executor.ExecutePaginationAsync(cmd, conn, request, MapToProduct);
        }

        public async Task<OperationResult<decimal>> GetDiscountPercentAsync(int productId)
        {
            await using var conn = _executor.CreateConnection();
            using var cmd = _executor.CreateCommand(conn, "usp_Products_GetDiscountPercent");

            cmd.Parameters.Add("@ProductId", SqlDbType.Int).Value = productId;
            return await _executor.ExecuteScalarAsync<decimal>(cmd, conn);
        }

        public async Task<OperationResult<Guid?>> GetImageGuidAsync(int productId)
        {
            await using var conn = _executor.CreateConnection();
            using var cmd = _executor.CreateCommand(conn, "usp_Products_GetImageGuid");

            cmd.Parameters.Add("@ProductId", SqlDbType.Int).Value = productId;
            return await _executor.ExecuteSingleAsync(cmd, conn, reader =>
            {
                var ordinal = reader.GetOrdinal("ImageGuid");
                return reader.IsDBNull(ordinal) ? (Guid?)null : reader.GetGuid(ordinal);
            });
        }

        public async Task<OperationResult<bool>> UpdateAsync(Product product)
        {
            await using var conn = _executor.CreateConnection();
            using var cmd = _executor.CreateCommand(conn, "usp_Products_Update");

            AddParameters(cmd, product, isUpdate: true);
            return await _executor.ExecuteBooleanOperationAsync(cmd, conn);
        }

        public async Task<OperationResult<bool>> ActivateAsync(int productId)
        {
            await using var conn = _executor.CreateConnection();
            using var cmd = _executor.CreateCommand(conn, "usp_Products_Activate");

            cmd.Parameters.Add("@ProductId", SqlDbType.Int).Value = productId;
            return await _executor.ExecuteBooleanOperationAsync(cmd, conn);
        }

        public async Task<OperationResult<bool>> DeactivateAsync(int productId)
        {
            await using var conn = _executor.CreateConnection();
            using var cmd = _executor.CreateCommand(conn, "usp_Products_Deactivate");

            cmd.Parameters.Add("@ProductId", SqlDbType.Int).Value = productId;
            return await _executor.ExecuteBooleanOperationAsync(cmd, conn);
        }

        private static Product MapToProduct(SqlDataReader reader)
        {
            var descOrdinal = reader.GetOrdinal("Description");
            string? desc = reader.IsDBNull(descOrdinal) ? null : reader.GetString(descOrdinal);

            var imageOrdinal = reader.GetOrdinal("ImageGuid");
            Guid? imageGuid = reader.IsDBNull(imageOrdinal) ? null : reader.GetGuid(imageOrdinal);

            var updatedAtOrdinal = reader.GetOrdinal("UpdatedAt");
            DateTime? updatedAt = reader.IsDBNull(updatedAtOrdinal) ? null : reader.GetDateTime(updatedAtOrdinal);

            return new Product(
                productId: reader.GetInt32(reader.GetOrdinal("ProductId")),
                categoryId: reader.GetInt32(reader.GetOrdinal("CategoryId")),
                productName: reader.GetString(reader.GetOrdinal("ProductName")),
                sku: reader.GetString(reader.GetOrdinal("SKU")),
                description: desc,
                unitId: reader.GetInt32(reader.GetOrdinal("UnitId")),
                costPrice: reader.GetDecimal(reader.GetOrdinal("CostPrice")),
                sellPrice: reader.GetDecimal(reader.GetOrdinal("SellPrice")),
                discountPercent: reader.GetDecimal(reader.GetOrdinal("DiscountPercent")),
                imageGuid: imageGuid,
                isActive: reader.GetBoolean(reader.GetOrdinal("IsActive")),
                createdAt: reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                updatedAt: updatedAt);
        }

        private static void AddParameters(SqlCommand cmd, Product product, bool isUpdate)
        {
            cmd.Parameters.Add("@CategoryId", SqlDbType.Int).Value = product.CategoryId;
            cmd.Parameters.Add("@ProductName", SqlDbType.NVarChar, 150).Value = product.ProductName;
            cmd.Parameters.Add("@SKU", SqlDbType.NVarChar, 50).Value = product.SKU;
            cmd.Parameters.Add("@Description", SqlDbType.NVarChar, 250).Value =
                product.Description ?? (object)DBNull.Value;
            cmd.Parameters.Add("@UnitId", SqlDbType.Int).Value = product.UnitId;
            cmd.Parameters.Add("@CostPrice", SqlDbType.Decimal).Value = product.CostPrice;
            cmd.Parameters.Add("@SellPrice", SqlDbType.Decimal).Value = product.SellPrice;
            cmd.Parameters.Add("@DiscountPercent", SqlDbType.Decimal).Value = product.DiscountPercent;
            cmd.Parameters.Add("@ImageGuid", SqlDbType.UniqueIdentifier).Value =
                product.ImageGuid ?? (object)DBNull.Value;

            if (isUpdate)
            {
                cmd.Parameters.Add("@ProductId", SqlDbType.Int).Value = product.ProductId;
                cmd.Parameters.Add("@IsActive", SqlDbType.Bit).Value = product.IsActive;
            }
        }
    }
}
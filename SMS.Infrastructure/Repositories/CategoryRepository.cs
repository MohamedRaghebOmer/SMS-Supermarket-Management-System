using Microsoft.Data.SqlClient;
using SMS.Application.Common.Results;
using SMS.Application.Interfaces.DataAccess;
using SMS.Application.Interfaces.Repositories;
using SMS.Domain.Entities;
using SMS.Shared.Common;
using System.Data;

namespace SMS.Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly IStoredProcedureExecutor _executor;

        public CategoryRepository(IStoredProcedureExecutor executor)
        {
            _executor = executor;
        }

        public async Task<OperationResult<int>> AddAsync(Category category)
        {
            await using var conn = _executor.CreateConnection();
            await using var cmd = _executor.CreateCommand(conn, "usp_Categories_Insert");

            AddParameters(cmd, category, isUpdate: false);

            var insertedIdParam = new SqlParameter("@InsertedId", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(insertedIdParam);

            return await _executor.ExecuteNonQueryAsync<int>(cmd, conn, insertedIdParam);
        }

        public async Task<OperationResult<Category?>> FindByIdAsync(int categoryId)
        {
            await using var conn = _executor.CreateConnection();
            await using var cmd = _executor.CreateCommand(conn, "usp_Categories_GetById");

            cmd.Parameters.Add("@CategoryId", SqlDbType.Int).Value = categoryId;
            return await _executor.ExecuteSingleAsync(cmd, conn, MapToCategory);
        }

        public async Task<OperationResult<Category?>> FindByNameAsync(string categoryName)
        {
            await using var conn = _executor.CreateConnection();
            await using var cmd = _executor.CreateCommand(conn, "usp_Categories_GetByName");

            cmd.Parameters.Add("@CategoryName", SqlDbType.NVarChar, 100).Value = categoryName;
            return await _executor.ExecuteSingleAsync(cmd, conn, MapToCategory);
        }

        public async Task<OperationResult<bool>> IsActive(int categoryId)
        {
            await using var conn = _executor.CreateConnection();
            await using var cmd = _executor.CreateCommand(conn, "usp_Categories_IsActive");

            cmd.Parameters.Add("@CategoryId", SqlDbType.Int).Value = categoryId;
            return await _executor.ExecuteBooleanOperationAsync(cmd, conn);
        }

        public async Task<OperationResult<PaginationResponse<Category>>> GetPagedAsync(PaginationRequest request)
        {
            await using var conn = _executor.CreateConnection();
            await using var cmd = _executor.CreateCommand(conn, "usp_Categories_GetPaged");

            return await _executor.ExecutePaginationAsync(cmd, conn, request, MapToCategory);
        }

        public async Task<OperationResult<PaginationResponse<Category>>> GetPagedByIsActiveAsync(
            PaginationRequest request, bool isActive)
        {
            await using var conn = _executor.CreateConnection();
            await using var cmd = _executor.CreateCommand(conn, "usp_Categories_GetPagedByIsActive");

            cmd.Parameters.Add("@IsActive", SqlDbType.Bit).Value = isActive;
            return await _executor.ExecutePaginationAsync(cmd, conn, request, MapToCategory);
        }

        public async Task<OperationResult<bool>> UpdateAsync(Category category)
        {
            await using var conn = _executor.CreateConnection();
            await using var cmd = _executor.CreateCommand(conn, "usp_Categories_Update");

            AddParameters(cmd, category, isUpdate: true);
            return await _executor.ExecuteBooleanOperationAsync(cmd, conn);
        }

        public async Task<OperationResult<bool>> ActivateAsync(int categoryId)
        {
            await using var conn = _executor.CreateConnection();
            await using var cmd = _executor.CreateCommand(conn, "usp_Categories_Activate");

            cmd.Parameters.Add("@CategoryId", SqlDbType.Int).Value = categoryId;
            return await _executor.ExecuteBooleanOperationAsync(cmd, conn);
        }

        public async Task<OperationResult<bool>> DeactivateAsync(int categoryId)
        {
            await using var conn = _executor.CreateConnection();
            await using var cmd = _executor.CreateCommand(conn, "usp_Categories_Deactivate");

            cmd.Parameters.Add("@CategoryId", SqlDbType.Int).Value = categoryId;
            return await _executor.ExecuteBooleanOperationAsync(cmd, conn);
        }


        private static Category MapToCategory(SqlDataReader reader)
        {
            var descOrdinal = reader.GetOrdinal("CategoryDescription");
            string? desc = reader.IsDBNull(descOrdinal) ? null : reader.GetString(descOrdinal);

            return new Category(
                categoryId: reader.GetInt32(reader.GetOrdinal("CategoryId")),
                categoryName: reader.GetString(reader.GetOrdinal("CategoryName")),
                categoryDescription: desc,
                isActive: reader.GetBoolean(reader.GetOrdinal("IsActive")));
        }

        private static void AddParameters(SqlCommand cmd, Category category, bool isUpdate)
        {
            cmd.Parameters.Add("@CategoryName", SqlDbType.NVarChar, 100).Value = category.CategoryName;
            cmd.Parameters.Add("@CategoryDescription", SqlDbType.NVarChar, 250).Value =
                category.CategoryDescription ?? (object)DBNull.Value;

            if (isUpdate)
            {
                cmd.Parameters.Add("@CategoryId", SqlDbType.Int).Value = category.CategoryId;
                cmd.Parameters.Add("@IsActive", SqlDbType.Bit).Value = category.IsActive;
            }
        }
    }
}
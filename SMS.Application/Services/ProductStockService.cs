using SMS.Application.Interfaces.Helpers;
using SMS.Application.Interfaces.Repositories;
using SMS.Application.Interfaces.Services;
using SMS.Domain.Entities;
using SMS.Shared.Common;
using SMS.Shared.Guards;

namespace SMS.Application.Services
{
    public class ProductStockService : IProductStockService
    {
        private readonly IProductStockRepository _repo;
        private readonly IValidationHelper _validationHelper;

        public ProductStockService(IProductStockRepository repo, IValidationHelper validationHelper)
        {
            _repo = repo;
            _validationHelper = validationHelper;
        }

        public async Task<PaginationResponse<ProductStock>> GetPagedAsync(PaginationRequest request)
        {
            _validationHelper.ValidatePagination(request);

            var result = await _repo.GetPagedAsync(request);
            result.ThrowIfNotSuccess();

            return result.Data!;
        }

        public async Task<ProductStock> GetByIdAsync(int productId)
        {
            NumericGuard.AgainstInvalidId(productId);

            var result = await _repo.FindByIdAsync(productId);
            result.ThrowIfNotSuccess();
            result.ThrowNotFoundIfDataNull();

            return result.Data!;
        }

        public async Task<decimal> GetQuantityOnHandAsync(int productId)
        {
            NumericGuard.AgainstInvalidId(productId);

            var result = await _repo.GetQuantityOnHandAsync(productId);
            result.ThrowIfNotSuccess();

            return result.Data;
        }

        public async Task<decimal> GetReorderLevelAsync(int productId)
        {
            NumericGuard.AgainstInvalidId(productId);

            var result = await _repo.GetReorderLevelAsync(productId);
            result.ThrowIfNotSuccess();

            return result.Data;
        }

        public async Task<bool> UpdateReorderLevelAsync(int productId, decimal reorderLevel)
        {
            NumericGuard.AgainstInvalidId(productId);
            NumericGuard.AgainstNegativeNumber(reorderLevel, nameof(reorderLevel));

            var result = await _repo.UpdateReorderLevelAsync(productId, reorderLevel);
            result.ThrowIfNotSuccess();

            return result.Data;
        }
    }
}

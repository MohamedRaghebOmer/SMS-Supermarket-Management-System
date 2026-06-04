using SMS.Application.Interfaces.Helpers;
using SMS.Application.Interfaces.Repositories;
using SMS.Application.Interfaces.Services;
using SMS.Application.Mapping;
using SMS.Contracts.Requests.Sales;
using SMS.Contracts.Responses;
using SMS.Domain.Entities;
using SMS.Shared.Common;
using SMS.Shared.Enums;
using SMS.Shared.Guards;

namespace SMS.Application.Services
{
    public class SaleService : ISaleService
    {
        private readonly ISaleRepository _repo;
        private readonly IValidationHelper _validationHelper;

        public SaleService(ISaleRepository repo, IValidationHelper validationHelper)
        {
            _repo = repo;
            _validationHelper = validationHelper;
        }

        public async Task<int> AddAsync(CreateSaleRequestDto dto, int cashierId)
        {
            ArgumentNullException.ThrowIfNull(dto);
            ValidateSaleAsync(dto);

            var result = await _repo.AddAsync(dto.ToEntity(cashierId));
            result.ThrowIfNotSuccess();

            return result.Data;
        }

        public async Task<SaleResponseDto> GetByIdAsync(int saleId)
        {
            NumericGuard.AgainstInvalidId(saleId);

            var result = await _repo.FindByIdAsync(saleId);
            result.ThrowIfNotSuccess();
            result.ThrowNotFoundIfDataNull();

            return result.Data!.ToDto();
        }

        public async Task<PaginationResponse<SaleResponseDto>> GetPagedAsync(PaginationRequest paginationRequest)
        {
            _validationHelper.ValidatePagination(paginationRequest);

            var result = await _repo.GetPagedAsync(paginationRequest);
            result.ThrowIfNotSuccess();

            return BuildPagedResponse(result, paginationRequest);
        }

        public async Task<PaginationResponse<SaleResponseDto>> GetPagedByCashierIdAsync(
            int cashierId, PaginationRequest paginationRequest)
        {
            NumericGuard.AgainstInvalidId(cashierId);
            _validationHelper.ValidatePagination(paginationRequest);

            var result = await _repo.GetPagedByCashierIdAsync(cashierId, paginationRequest);
            result.ThrowIfNotSuccess();

            return BuildPagedResponse(result, paginationRequest);
        }

        public async Task<PaginationResponse<SaleResponseDto>> GetPagedByCustomerIdAsync(
            int customerId, PaginationRequest paginationRequest)
        {
            NumericGuard.AgainstInvalidId(customerId);
            _validationHelper.ValidatePagination(paginationRequest);

            var result = await _repo.GetPagedByCustomerIdAsync(customerId, paginationRequest);
            result.ThrowIfNotSuccess();

            return BuildPagedResponse(result, paginationRequest);
        }

        public async Task<PaginationResponse<SaleResponseDto>> GetPagedByDateRangeAsync(
            DateTime startDate, DateTime endDate, PaginationRequest paginationRequest)
        {
            DateGuard.AgainstInvalidDateRange(startDate, endDate, nameof(startDate), nameof(endDate));
            _validationHelper.ValidatePagination(paginationRequest);

            var result = await _repo.GetPagedByDateRangeAsync(startDate, endDate, paginationRequest);
            result.ThrowIfNotSuccess();

            return BuildPagedResponse(result, paginationRequest);
        }

        public async Task<bool> ExistsByIdAsync(int saleId)
        {
            NumericGuard.AgainstInvalidId(saleId);

            var result = await _repo.ExistsByIdAsync(saleId);
            result.ThrowIfNotSuccess();

            return result.Data;
        }


        private void ValidateSaleAsync(CreateSaleRequestDto dto)
        {
            NumericGuard.AgainstInvalidId(dto.CustomerId);
            NumericGuard.AgainstNegativeNumber(dto.SubTotal, nameof(dto.SubTotal));
            NumericGuard.AgainstNegativeNumber(dto.DiscountAmount, nameof(dto.DiscountAmount));
            NumericGuard.AgainstNegativeNumber(dto.NetTotal, nameof(dto.NetTotal));
            NumericGuard.AgainstNegativeNumber(dto.PaidAmount, nameof(dto.PaidAmount));
            NumericGuard.AgainstNegativeNumber(dto.ChangeAmount, nameof(dto.ChangeAmount));

            if (dto.DiscountAmount > dto.SubTotal)
            {
                throw new ArgumentException("DiscountAmount cannot exceed SubTotal.", nameof(dto.DiscountAmount));
            }

            if (dto.NetTotal != dto.SubTotal - dto.DiscountAmount)
            {
                throw new ArgumentException("NetTotal must equal SubTotal minus DiscountAmount.", nameof(dto.NetTotal));
            }

            if (dto.PaidAmount < dto.NetTotal && !dto.IsCredit)
            {
                throw new ArgumentException("PaidAmount cannot be less than NetTotal for non-credit sales.",
                    nameof(dto.PaidAmount));
            }

            if (!dto.IsCredit && dto.ChangeAmount != dto.PaidAmount - dto.NetTotal)
            {
                throw new ArgumentException("ChangeAmount must equal PaidAmount minus NetTotal.",
                    nameof(dto.ChangeAmount));
            }

            if (dto is { IsCredit: true, CustomerId: null })
            {
                throw new ArgumentException("CustomerId is required for credit sales.", nameof(dto.CustomerId));
            }

            if (dto is { IsCredit: false, PaymentMethod: null })
            {
                throw new ArgumentException("PaymentMethod is required for non-credit sales.",
                    nameof(dto.PaymentMethod));
            }

            if (dto.PaymentMethod.HasValue)
            {
                _validationHelper.ValidateEnum(dto.PaymentMethod.Value, typeof(PaymentMethod));
            }
        }

        private static PaginationResponse<SaleResponseDto> BuildPagedResponse(
            Common.Results.OperationResult<PaginationResponse<Sale>> result,
            PaginationRequest paginationRequest)
        {
            return new PaginationResponse<SaleResponseDto>
            {
                Items = result.Data!.Items.Select(sale => sale.ToDto()).ToList(),
                TotalCount = result.Data.TotalCount,
                Page = paginationRequest.Page,
                PageSize = paginationRequest.PageSize
            };
        }
    }
}
using SMS.Application.Common.Results;
using SMS.Application.Interfaces.Helpers;
using SMS.Application.Interfaces.Repositories;
using SMS.Application.Interfaces.Services;
using SMS.Application.Mapping;
using SMS.Contracts.Requests.Returns;
using SMS.Contracts.Responses;
using SMS.Domain.Entities;
using SMS.Shared.Common;
using SMS.Shared.Guards;

namespace SMS.Application.Services
{
    public class ReturnService : IReturnService
    {
        private readonly IReturnRepository _repo;
        private readonly IValidationHelper _validationHelper;

        public ReturnService(IReturnRepository repo, IValidationHelper validationHelper)
        {
            _repo = repo;
            _validationHelper = validationHelper;
        }

        public async Task<int> AddAsync(CreateReturnRequestDto dto, int createdBy)
        {
            ArgumentNullException.ThrowIfNull(dto);

            var result = await _repo.AddAsync(dto.ToEntity(createdBy));
            result.ThrowIfNotSuccess();

            return result.Data;
        }

        public async Task<ReturnResponseDto> GetByIdAsync(int returnId)
        {
            NumericGuard.AgainstInvalidId(returnId);

            var result = await _repo.FindByIdAsync(returnId);
            result.ThrowIfNotSuccess();
            result.ThrowNotFoundIfDataNull();

            return result.Data!.ToDto();
        }

        public async Task<PaginationResponse<ReturnResponseDto>> GetPagedAsync(PaginationRequest paginationRequest)
        {
            _validationHelper.ValidatePagination(paginationRequest);

            var result = await _repo.GetPagedAsync(paginationRequest);
            result.ThrowIfNotSuccess();

            return BuildPagedResponse(result, paginationRequest);
        }

        public async Task<IReadOnlyList<ReturnResponseDto>> GetBySaleIdAsync(int saleId)
        {
            NumericGuard.AgainstInvalidId(saleId);

            var result = await _repo.GetBySaleIdAsync(saleId);
            result.ThrowIfNotSuccess();

            return result.Data!.Select(r => r.ToDto()).ToList();
        }

        public async Task<PaginationResponse<ReturnResponseDto>> GetPagedByCustomerIdAsync(int customerId, PaginationRequest paginationRequest)
        {
            NumericGuard.AgainstInvalidId(customerId);
            _validationHelper.ValidatePagination(paginationRequest);

            var result = await _repo.GetPagedByCustomerIdAsync(customerId, paginationRequest);
            result.ThrowIfNotSuccess();

            return BuildPagedResponse(result, paginationRequest);
        }

        public async Task<PaginationResponse<ReturnResponseDto>> GetPagedByReturnTotalRangeAsync(decimal minTotal, decimal maxTotal, PaginationRequest paginationRequest)
        {
            NumericGuard.AgainstNegativeNumber(minTotal, nameof(minTotal));
            NumericGuard.AgainstNegativeNumber(maxTotal, nameof(maxTotal));
            if (minTotal > maxTotal)
            {
                throw new ArgumentException("minTotal cannot be greater than maxTotal.", nameof(minTotal));
            }

            _validationHelper.ValidatePagination(paginationRequest);

            var result = await _repo.GetPagedByReturnTotalRangeAsync(minTotal, maxTotal, paginationRequest);
            result.ThrowIfNotSuccess();

            return BuildPagedResponse(result, paginationRequest);
        }

        public async Task<decimal> GetReturnTotalByIdAsync(int returnId)
        {
            NumericGuard.AgainstInvalidId(returnId);

            var result = await _repo.GetReturnTotalByIdAsync(returnId);
            result.ThrowIfNotSuccess();

            return result.Data;
        }

        public async Task<PaginationResponse<ReturnResponseDto>> GetPagedByDateRangeAsync(DateTime? startDate, DateTime? endDate, PaginationRequest paginationRequest)
        {
            DateGuard.AgainstInvalidDateRange(startDate, endDate, nameof(startDate), nameof(endDate));
            _validationHelper.ValidatePagination(paginationRequest);

            var result = await _repo.GetPagedByDateRangeAsync(startDate, endDate, paginationRequest);
            result.ThrowIfNotSuccess();

            return BuildPagedResponse(result, paginationRequest);
        }

        private static PaginationResponse<ReturnResponseDto> BuildPagedResponse(
            OperationResult<PaginationResponse<Return>> result,
            PaginationRequest paginationRequest)
        {
            return new PaginationResponse<ReturnResponseDto>
            {
                Items = result.Data!.Items.Select(r => r.ToDto()).ToList(),
                TotalCount = result.Data.TotalCount,
                Page = paginationRequest.Page,
                PageSize = paginationRequest.PageSize
            };
        }
    }
}

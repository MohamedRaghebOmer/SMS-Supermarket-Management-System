using SMS.Application.Interfaces.Helpers;
using SMS.Application.Interfaces.Repositories;
using SMS.Application.Interfaces.Services;
using SMS.Application.Mapping;
using SMS.Contracts.Requests.Customers;
using SMS.Contracts.Responses;
using SMS.Domain.Entities;
using SMS.Shared.Common;
using SMS.Shared.Guards;

namespace SMS.Application.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _repo;
        private readonly IValidationHelper _validationHelper;

        public CustomerService(ICustomerRepository repo, IValidationHelper validationHelper)
        {
            _repo = repo;
            _validationHelper = validationHelper;
        }

        public async Task<int> AddAsync(CreateCustomerRequestDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);
            ValidateDto(dto);

            var result = await _repo.AddAsync(dto.ToEntity());
            result.ThrowIfNotSuccess();

            return result.Data;
        }

        public async Task<CustomerResponseDto> GetByIdAsync(int customerId)
        {
            NumericGuard.AgainstInvalidId(customerId);

            var result = await _repo.FindByIdAsync(customerId);
            result.ThrowIfNotSuccess();
            result.ThrowNotFoundIfDataNull();

            return result.Data!.ToDto();
        }

        public async Task<CustomerResponseDto> GetByPersonIdAsync(int personId)
        {
            NumericGuard.AgainstInvalidId(personId);

            var result = await _repo.FindByPersonIdAsync(personId);
            result.ThrowIfNotSuccess();
            result.ThrowNotFoundIfDataNull();

            return result.Data!.ToDto();
        }

        public async Task<PaginationResponse<CustomerResponseDto>> GetPagedAsync(
            PaginationRequest paginationRequest)
        {
            _validationHelper.ValidatePagination(paginationRequest);

            var result = await _repo.GetPagedAsync(paginationRequest);
            result.ThrowIfNotSuccess();

            return BuildPagedResponse(result, paginationRequest);
        }

        public async Task<PaginationResponse<CustomerResponseDto>> GetPagedActiveAsync(
            PaginationRequest paginationRequest)
        {
            _validationHelper.ValidatePagination(paginationRequest);

            var result = await _repo.GetPagedActiveAsync(paginationRequest);
            result.ThrowIfNotSuccess();

            return BuildPagedResponse(result, paginationRequest);
        }

        public async Task<bool> ExistsByIdAsync(int customerId)
        {
            NumericGuard.AgainstInvalidId(customerId);

            var result = await _repo.ExistsByIdAsync(customerId);
            result.ThrowIfNotSuccess();

            return result.Data;
        }

        public async Task<bool> ExistsByPersonIdAsync(int personId)
        {
            NumericGuard.AgainstInvalidId(personId);

            var result = await _repo.ExistsByPersonIdAsync(personId);
            result.ThrowIfNotSuccess();

            return result.Data;
        }

        public async Task<decimal> GetDebitAmountAsync(int customerId)
        {
            NumericGuard.AgainstInvalidId(customerId);

            var result = await _repo.GetDebitAmountAsync(customerId);
            result.ThrowIfNotSuccess();

            return result.Data;
        }

        /// <summary>
        /// Determines whether the specified customer is blocked.
        /// </summary>
        public async Task<bool> IsBlockedAsync(int customerId)
        {
            NumericGuard.AgainstInvalidId(customerId);

            var result = await _repo.IsBlocked(customerId);
            result.ThrowIfNotSuccess();

            return result.Data;
        }

        public async Task<bool> UpdateAsync(int customerId, UpdateCustomerRequestDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);
            NumericGuard.AgainstInvalidId(customerId);
            ValidateDto(dto);

            var result = await _repo.UpdateAsync(dto.ToEntity(customerId));
            result.ThrowIfNotSuccess();

            return result.Data;
        }

        public async Task<bool> DeactivateAsync(int customerId)
        {
            NumericGuard.AgainstInvalidId(customerId);

            var result = await _repo.DeactivateAsync(customerId);
            result.ThrowIfNotSuccess();

            return result.Data;
        }


        private static void ValidateDto(CreateCustomerRequestDto dto)
        {
            NumericGuard.AgainstInvalidId(dto.PersonId);
            if (dto.PaymentDay is < 1 or > 31)
            {
                throw new ArgumentOutOfRangeException(nameof(dto.PaymentDay), "PaymentDay must be between 1 and 31.");
            }

            if (!string.IsNullOrWhiteSpace(dto.Notes))
            {
                StringGuard.EnsureLengthInRange(dto.Notes, 1, 250, nameof(dto.Notes));
            }
        }

        private static void ValidateDto(UpdateCustomerRequestDto dto)
        {
            NumericGuard.AgainstInvalidId(dto.PersonId);
            if (dto.PaymentDay is < 1 or > 31)
            {
                throw new ArgumentOutOfRangeException(nameof(dto.PaymentDay), "PaymentDay must be between 1 and 31.");
            }

            if (!string.IsNullOrWhiteSpace(dto.Notes))
            {
                StringGuard.EnsureLengthInRange(dto.Notes, 1, 250, nameof(dto.Notes));
            }
        }

        private static PaginationResponse<CustomerResponseDto> BuildPagedResponse(
            Common.Results.OperationResult<PaginationResponse<Customer>> result,
            PaginationRequest paginationRequest)
        {
            return new PaginationResponse<CustomerResponseDto>
            {
                Items = result.Data!.Items.Select(customer => customer.ToDto()).ToList(),
                TotalCount = result.Data.TotalCount,
                Page = paginationRequest.Page,
                PageSize = paginationRequest.PageSize
            };
        }
    }
}
using SMS.Application.Interfaces.Helpers;
using SMS.Application.Interfaces.Repositories;
using SMS.Application.Interfaces.Services;
using SMS.Application.Mapping;
using SMS.Contracts.Requests.CustomerLedgers;
using SMS.Contracts.Responses;
using SMS.Shared.Common;
using SMS.Shared.Enums;
using SMS.Shared.Guards;

namespace SMS.Application.Services
{
    public class CustomerLedgerService : ICustomerLedgerService
    {
        private readonly ICustomerLedgerRepository _repo;
        private readonly IValidationHelper _validationHelper;

        public CustomerLedgerService(ICustomerLedgerRepository repo, IValidationHelper validationHelper)
        {
            _repo = repo;
            _validationHelper = validationHelper;
        }

        public async Task<int> AddAsync(CreateCustomerLedgerRequestDto dto, int userId)
        {
            ArgumentNullException.ThrowIfNull(dto);
            ValidateDto(dto);

            if (dto.DebitAmount == 0 && dto.CreditAmount == 0)
                throw new ArgumentException("Either DebitAmount or CreditAmount must be greater than zero.");

            if (dto.DebitAmount > 0 && dto.CreditAmount > 0)
                throw new ArgumentException("Only one of DebitAmount or CreditAmount can be greater than zero.");

            if (dto.EntryType == CustomerLedgerEntryType.Fee && dto.DebitAmount == 0)
                throw new ArgumentException("DebitAmount must be greater than zero for Fee entry type.");

            if (dto.EntryType == CustomerLedgerEntryType.Return
                && dto.CreditAmount == 0)
                throw new ArgumentException("CreditAmount must be greater than zero for return entry types.");

            if (dto.EntryType == CustomerLedgerEntryType.Payment && dto.CreditAmount == 0)
                throw new ArgumentException("CreditAmount must be greater than zero for payment entry type.");

            if (dto.EntryType == CustomerLedgerEntryType.Sale && dto.DebitAmount == 0)
                throw new ArgumentException("DebitAmount must be greater that zero for sale entry type.");

            _validationHelper.ValidateEnum(dto.EntryType, typeof(CustomerLedgerEntryType));
            _validationHelper.ValidateEnum(dto.ReferenceType, typeof(CustomerLedgerReferenceType));

            var result = await _repo.AddAsync(dto.ToEntity(userId));
            result.ThrowIfNotSuccess();

            return result.Data;
        }

        public async Task<CustomerLedgerResponseDto> GetByIdAsync(int ledgerId)
        {
            NumericGuard.AgainstInvalidId(ledgerId);

            var result = await _repo.FindByIdAsync(ledgerId);
            result.ThrowIfNotSuccess();
            result.ThrowNotFoundIfDataNull();

            return result.Data!.ToDto();
        }

        public async Task<PaginationResponse<CustomerLedgerResponseDto>> GetPagedAsync(
            PaginationRequest paginationRequest)
        {
            _validationHelper.ValidatePagination(paginationRequest);

            var result = await _repo.GetPagedAsync(paginationRequest);
            result.ThrowIfNotSuccess();

            return BuildPagedResponse(result, paginationRequest);
        }

        public async Task<PaginationResponse<CustomerLedgerResponseDto>> GetPagedByCustomerIdAsync(
            int customerId, PaginationRequest paginationRequest)
        {
            NumericGuard.AgainstInvalidId(customerId);
            _validationHelper.ValidatePagination(paginationRequest);

            var result = await _repo.GetPagedByCustomerIdAsync(customerId, paginationRequest);
            result.ThrowIfNotSuccess();

            return BuildPagedResponse(result, paginationRequest);
        }

        public async Task<bool> ExistsByIdAsync(int ledgerId)
        {
            NumericGuard.AgainstInvalidId(ledgerId);

            var result = await _repo.ExistsByIdAsync(ledgerId);
            result.ThrowIfNotSuccess();

            return result.Data;
        }

        public async Task<PaginationResponse<CustomerLedgerResponseDto>> GetPagedByEntryTypeAsync(
            CustomerLedgerEntryType entryType, PaginationRequest paginationRequest)
        {
            _validationHelper.ValidatePagination(paginationRequest);

            var result = await _repo.GetPagedByEntryTypeAsync(entryType, paginationRequest);
            result.ThrowIfNotSuccess();

            return BuildPagedResponse(result, paginationRequest);
        }

        public async Task<PaginationResponse<CustomerLedgerResponseDto>> GetPagedByReferenceTypeAsync(
            CustomerLedgerReferenceType referenceType, PaginationRequest paginationRequest)
        {
            _validationHelper.ValidatePagination(paginationRequest);

            var result = await _repo.GetPagedByReferenceTypeAsync(referenceType, paginationRequest);
            result.ThrowIfNotSuccess();

            return BuildPagedResponse(result, paginationRequest);
        }

        public async Task<PaginationResponse<CustomerLedgerResponseDto>> GetPagedByCreatedByAsync(
            int userId, PaginationRequest paginationRequest)
        {
            NumericGuard.AgainstInvalidId(userId);
            _validationHelper.ValidatePagination(paginationRequest);

            var result = await _repo.GetPagedByCreatedByAsync(userId, paginationRequest);
            result.ThrowIfNotSuccess();

            return BuildPagedResponse(result, paginationRequest);
        }


        private static void ValidateDto(CreateCustomerLedgerRequestDto dto)
        {
            NumericGuard.AgainstInvalidId(dto.CustomerId);
            NumericGuard.AgainstInvalidId(dto.ReferenceId);
            NumericGuard.AgainstNegativeNumber(dto.DebitAmount, nameof(dto.DebitAmount));
            NumericGuard.AgainstNegativeNumber(dto.CreditAmount, nameof(dto.CreditAmount));
        }


        private static PaginationResponse<CustomerLedgerResponseDto> BuildPagedResponse(
            Common.Results.OperationResult<PaginationResponse<Domain.Entities.CustomerLedger>> result,
            PaginationRequest paginationRequest)
        {
            return new PaginationResponse<CustomerLedgerResponseDto>
            {
                Items = result.Data.Items.Select(ledger => ledger.ToDto()).ToList(),
                TotalCount = result.Data.TotalCount,
                Page = paginationRequest.Page,
                PageSize = paginationRequest.PageSize
            };
        }
    }
}

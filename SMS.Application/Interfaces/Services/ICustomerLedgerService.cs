using SMS.Contracts.Requests.CustomerLedgers;
using SMS.Contracts.Responses;
using SMS.Shared.Common;
using SMS.Shared.Enums;

namespace SMS.Application.Interfaces.Services
{
    public interface ICustomerLedgerService
    {
        Task<int> AddAsync(CreateCustomerLedgerRequestDto dto, int userId);
        Task<CustomerLedgerResponseDto> GetByIdAsync(int ledgerId);
        Task<PaginationResponse<CustomerLedgerResponseDto>> GetPagedAsync(
            PaginationRequest paginationRequest);
        Task<PaginationResponse<CustomerLedgerResponseDto>> GetPagedByCustomerIdAsync(
            int customerId,
            PaginationRequest paginationRequest);
        Task<bool> ExistsByIdAsync(int ledgerId);
        Task<PaginationResponse<CustomerLedgerResponseDto>> GetPagedByEntryTypeAsync(
            CustomerLedgerEntryType entryType,
            PaginationRequest paginationRequest);
        Task<PaginationResponse<CustomerLedgerResponseDto>> GetPagedByReferenceTypeAsync(
            CustomerLedgerReferenceType referenceType,
            PaginationRequest paginationRequest);
        Task<PaginationResponse<CustomerLedgerResponseDto>> GetPagedByCreatedByAsync(
            int userId,
            PaginationRequest paginationRequest);
    }
}

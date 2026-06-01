using SMS.Application.Common.Results;
using SMS.Domain.Entities;
using SMS.Shared.Common;
using SMS.Shared.Enums;

namespace SMS.Application.Interfaces.Repositories
{
    public interface ICustomerLedgerRepository
    {
        Task<OperationResult<int>> AddAsync(CustomerLedger ledger);
        Task<OperationResult<CustomerLedger?>> FindByIdAsync(int ledgerId);
        Task<OperationResult<PaginationResponse<CustomerLedger>>> GetPagedAsync(
            PaginationRequest paginationRequest);
        Task<OperationResult<PaginationResponse<CustomerLedger>>> GetPagedByCustomerIdAsync(
            int customerId, PaginationRequest paginationRequest);
        Task<OperationResult<PaginationResponse<CustomerLedger>>> GetPagedByEntryTypeAsync(
            CustomerLedgerEntryType entryType, PaginationRequest paginationRequest);
        Task<OperationResult<PaginationResponse<CustomerLedger>>> GetPagedByReferenceTypeAsync(
            CustomerLedgerReferenceType referenceType, PaginationRequest paginationRequest);
        Task<OperationResult<PaginationResponse<CustomerLedger>>> GetPagedByCreatedByAsync(
            int createdBy, PaginationRequest paginationRequest);
        Task<OperationResult<bool>> ExistsByIdAsync(int ledgerId);
    }
}

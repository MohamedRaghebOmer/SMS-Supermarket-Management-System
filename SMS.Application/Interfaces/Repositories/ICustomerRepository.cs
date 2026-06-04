using SMS.Application.Common.Results;
using SMS.Domain.Entities;
using SMS.Shared.Common;

namespace SMS.Application.Interfaces.Repositories
{
    public interface ICustomerRepository
    {
        Task<OperationResult<int>> AddAsync(Customer customer);
        Task<OperationResult<Customer?>> FindByIdAsync(int customerId);
        Task<OperationResult<Customer?>> FindByPersonIdAsync(int personId);
        Task<OperationResult<PaginationResponse<Customer>>> GetPagedAsync(PaginationRequest paginationRequest);
        Task<OperationResult<PaginationResponse<Customer>>> GetPagedActiveAsync(PaginationRequest paginationRequest);
        Task<OperationResult<bool>> ExistsByIdAsync(int customerId);
        Task<OperationResult<bool>> ExistsByPersonIdAsync(int personId);
        Task<OperationResult<decimal>> GetDebitAmountAsync(int customerId);

        /// <summary>
        /// Determines whether the specified customer is blocked.
        /// </summary>
        Task<OperationResult<bool>> IsBlocked(int customerId);

        Task<OperationResult<bool>> UpdateAsync(Customer customer);
        Task<OperationResult<bool>> DeactivateAsync(int customerId);
    }
}
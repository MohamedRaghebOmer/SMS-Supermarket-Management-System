using SMS.Contracts.Requests.Customers;
using SMS.Contracts.Responses;
using SMS.Shared.Common;

namespace SMS.Application.Interfaces.Services
{
    public interface ICustomerService
    {
        Task<int> AddAsync(CreateCustomerRequestDto dto);
        Task<CustomerResponseDto> GetByIdAsync(int customerId);
        Task<CustomerResponseDto> GetByPersonIdAsync(int personId);
        Task<PaginationResponse<CustomerResponseDto>> GetPagedAsync(PaginationRequest paginationRequest);
        Task<PaginationResponse<CustomerResponseDto>> GetPagedActiveAsync(PaginationRequest paginationRequest);
        Task<bool> ExistsByIdAsync(int customerId);
        Task<bool> ExistsByPersonIdAsync(int personId);
        /// <summary>
        /// Determines whether the specified customer is blocked.
        /// </summary>
        Task<bool> IsBlockedAsync(int customerId);
        Task<bool> UpdateAsync(int customerId, UpdateCustomerRequestDto dto);
        Task<bool> DeleteAsync(int customerId);
    }
}

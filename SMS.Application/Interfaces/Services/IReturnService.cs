using SMS.Contracts.Requests.Returns;
using SMS.Contracts.Responses;
using SMS.Shared.Common;

namespace SMS.Application.Interfaces.Services
{
    public interface IReturnService
    {
        Task<int> AddAsync(CreateReturnRequestDto dto, int createdBy);
        Task<ReturnResponseDto> GetByIdAsync(int returnId);
        Task<PaginationResponse<ReturnResponseDto>> GetPagedAsync(PaginationRequest paginationRequest);
        Task<IReadOnlyList<ReturnResponseDto>> GetBySaleIdAsync(int saleId);
        Task<PaginationResponse<ReturnResponseDto>> GetPagedByCustomerIdAsync(int customerId, PaginationRequest paginationRequest);
        Task<PaginationResponse<ReturnResponseDto>> GetPagedByReturnTotalRangeAsync(decimal minTotal, decimal maxTotal, PaginationRequest paginationRequest);
        Task<decimal> GetReturnTotalByIdAsync(int returnId);
        Task<PaginationResponse<ReturnResponseDto>> GetPagedByDateRangeAsync(DateTime? startDate, DateTime? endDate, PaginationRequest paginationRequest);
    }
}

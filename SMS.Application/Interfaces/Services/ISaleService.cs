using SMS.Contracts.Requests.Sales;
using SMS.Contracts.Responses;
using SMS.Shared.Common;

namespace SMS.Application.Interfaces.Services
{
    public interface ISaleService
    {
        Task<int> AddAsync(CreateSaleRequestDto dto, int cashierId);
        Task<SaleResponseDto> GetByIdAsync(int saleId);
        Task<PaginationResponse<SaleResponseDto>> GetPagedAsync(PaginationRequest paginationRequest);

        Task<PaginationResponse<SaleResponseDto>> GetPagedByCashierIdAsync(
            int cashierId, PaginationRequest paginationRequest);

        Task<PaginationResponse<SaleResponseDto>> GetPagedByCustomerIdAsync(int customerId,
            PaginationRequest paginationRequest);

        Task<PaginationResponse<SaleResponseDto>> GetPagedByDateRangeAsync(DateTime startDate, DateTime endDate,
            PaginationRequest paginationRequest);

        Task<bool> ExistsByIdAsync(int saleId);
    }
}
using SMS.Contracts.Requests.ReturnItems;
using SMS.Contracts.Responses;
using SMS.Shared.Common;

namespace SMS.Application.Interfaces.Services
{
    public interface IReturnItemService
    {
        Task<int> AddAsync(CreateReturnItemRequestDto dto);
        Task<ReturnItemResponseDto> GetByIdAsync(int returnItemId);
        Task<PaginationResponse<ReturnItemResponseDto>> GetPagedAsync(PaginationRequest request);
        Task<PaginationResponse<ReturnItemResponseDto>> GetPagedByReturnIdAsync(int returnId, PaginationRequest request);
        Task<PaginationResponse<ReturnItemResponseDto>> GetPagedByProductIdAsync(int productId, PaginationRequest request);
    }
}

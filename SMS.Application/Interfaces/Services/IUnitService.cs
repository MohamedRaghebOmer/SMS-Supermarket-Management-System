using SMS.Contracts.Requests.Units;
using SMS.Contracts.Responses;
using SMS.Shared.Common;

namespace SMS.Application.Interfaces.Services
{
    public interface IUnitService
    {
        Task<int> AddAsync(CreateUnitRequestDto dto);
        Task<UnitResponseDto> GetByIdAsync(int unitId);
        Task<UnitResponseDto> GetByNameAsync(string unitName);
        Task<UnitResponseDto> GetBySymbolAsync(string symbol);
        Task<PaginationResponse<UnitResponseDto>> GetPagedByIsDecimalAsync(PaginationRequest request, bool isDecimal);
        Task<bool> UpdateAsync(int unitId, UpdateUnitRequestDto dto);
    }
}
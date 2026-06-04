using SMS.Application.Interfaces.Helpers;
using SMS.Application.Interfaces.Repositories;
using SMS.Application.Interfaces.Services;
using SMS.Application.Mapping;
using SMS.Contracts.Requests.Units;
using SMS.Contracts.Responses;
using SMS.Shared.Common;
using SMS.Shared.Guards;

namespace SMS.Application.Services
{
    public class UnitService : IUnitService
    {
        private readonly IUnitRepository _repo;
        private readonly IValidationHelper _validationHelper;

        public UnitService(IUnitRepository repo, IValidationHelper validationHelper)
        {
            _repo = repo;
            _validationHelper = validationHelper;
        }

        public async Task<int> AddAsync(CreateUnitRequestDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);
            var result = await _repo.AddAsync(dto.ToEntity());
            result.ThrowIfNotSuccess();

            return result.Data;
        }

        public async Task<UnitResponseDto> GetByIdAsync(int unitId)
        {
            NumericGuard.AgainstInvalidId(unitId);

            var result = await _repo.FindByIdAsync(unitId);
            result.ThrowIfNotSuccess();

            return result.Data!.ToDto();
        }

        public async Task<UnitResponseDto> GetByNameAsync(string unitName)
        {
            StringGuard.AgainstNullOrWhiteSpace(unitName, nameof(unitName));

            var result = await _repo.FindByNameAsync(unitName);
            result.ThrowIfNotSuccess();
            result.ThrowNotFoundIfDataNull();

            return result.Data!.ToDto();
        }

        public async Task<UnitResponseDto> GetBySymbolAsync(string symbol)
        {
            StringGuard.AgainstNullOrWhiteSpace(symbol, nameof(symbol));

            var result = await _repo.FindBySymbolAsync(symbol);
            result.ThrowIfNotSuccess();
            result.ThrowNotFoundIfDataNull();

            return result.Data!.ToDto();
        }

        public async Task<PaginationResponse<UnitResponseDto>> GetPagedByIsDecimalAsync(PaginationRequest request,
            bool isDecimal)
        {
            _validationHelper.ValidatePagination(request);

            var result = await _repo.GetPagedByIsDecimalAsync(request, isDecimal);
            result.ThrowIfNotSuccess();

            return new PaginationResponse<UnitResponseDto>
            {
                Items = result.Data!.Items.Select(u => u.ToDto()).ToList(),
                TotalCount = result.Data.TotalCount,
                Page = result.Data.Page,
                PageSize = result.Data.PageSize
            };
        }

        public async Task<bool> UpdateAsync(int unitId, UpdateUnitRequestDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);

            var result = await _repo.UpdateAsync(dto.ToEntity(unitId));
            result.ThrowIfNotSuccess();

            return result.Data;
        }
    }
}
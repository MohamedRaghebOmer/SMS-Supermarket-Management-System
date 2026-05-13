using SMS.Application.Interfaces.Repositories;
using SMS.Application.Interfaces.Services;
using SMS.Application.Mapping;
using SMS.Contracts.Requests.Countries;
using SMS.Contracts.Responses;
using SMS.Shared.Common;
using SMS.Shared.Guards;

namespace SMS.Application.Services
{
    public class CountryService : ICountryService
    {
        private readonly ICountryRepository _repo;

        public CountryService(ICountryRepository repo)
        {
            _repo = repo;
        }


        public async Task<int> AddAsync(CreateCountryRequestDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);
            StringGuard.AgainstNullOrEmptyString(dto.CountryName, dto.CountryName);

            var result = await _repo.AddAsync(dto.ToEntity());
            result.ThrowIfNotSuccess();

            return result.Data;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            NumericGuard.AgainstInvalidId(id);

            var result = await _repo.ExistsAsync(id);
            result.ThrowIfNotSuccess();
            return result.Data;
        }

        public async Task<bool> ExistsAsync(string countryName)
        {
            StringGuard.AgainstNullOrEmptyString(countryName, "Country name");

            var result = await _repo.ExistsAsync(countryName);
            result.ThrowIfNotSuccess();

            return result.Data;
        }

        public async Task<CountryResponseDto> GetAsync(int id)
        {
            NumericGuard.AgainstInvalidId(id);

            var result = await _repo.FindAsync(id);

            result.ThrowIfNotSuccess();
            result.ThrowNotFoundIfDataNull();

            return result.Data.ToDto();
        }

        public async Task<CountryResponseDto> GetAsync(string countryName)
        {
            StringGuard.AgainstNullOrEmptyString(countryName, "Country name");

            var result = await _repo.FindByNameAsync(countryName);

            result.ThrowIfNotSuccess();
            result.ThrowNotFoundIfDataNull();

            return result.Data.ToDto();
        }

        public async Task<List<CountryResponseDto>> GetAllAsync()
        {
            var result = await _repo.GetAllAsync();
            result.ThrowIfNotSuccess();
            return result.Data.Select(c => c.ToDto()).ToList();
        }

        public async Task<PaginationResponse<CountryResponseDto>> GetPagedAsync(PaginationRequest paginationRequest)
        {
            ArgumentNullException.ThrowIfNull(paginationRequest);
            NumericGuard.AgainstInvalidId(paginationRequest.Page);
            NumericGuard.AgainstInvalidId(paginationRequest.PageSize);

            var result = await _repo.GetPagedAsync(paginationRequest);
            result.ThrowIfNotSuccess();

            return new PaginationResponse<CountryResponseDto>
            {
                Items = result.Data.Items.Select(c => c.ToDto()).ToList(),
                TotalCount = result.Data.TotalCount,
                Page = paginationRequest.Page,
                PageSize = paginationRequest.PageSize
            };
        }

        public async Task<bool> UpdateAsync(int countryId, UpdateCountryRequestDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);
            NumericGuard.AgainstInvalidId(countryId);
            StringGuard.AgainstNullOrEmptyString(dto.CountryName, "Country name");

            var result = await _repo.UpdateAsync(dto.ToEntity());
            result.ThrowIfNotSuccess();

            return result.Data;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            NumericGuard.AgainstInvalidId(id);
            var result = await _repo.DeleteAsync(id);
            result.ThrowIfNotSuccess();
            return result.Data;
        }
    }
}

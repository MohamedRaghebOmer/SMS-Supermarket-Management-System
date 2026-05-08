using SMS.Application.Exceptions;
using SMS.Application.Interfaces.Repositories;
using SMS.Application.Interfaces.Services;
using SMS.Application.Mapping;
using SMS.Contracts.Common;
using SMS.Contracts.Requests.Countries;
using SMS.Contracts.Responses;
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
            StringGuard.AgainstNullOrEmptyString(dto.CountryName, "Country name");

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

        public async Task<CountryResponseDto> FindAsync(int id)
        {
            NumericGuard.AgainstInvalidId(id);

            var result = await _repo.GetAsync(id);
            result.ThrowIfNotSuccess();

            if (result.Data is null)
            {
                throw new NotFoundException($"Country with Id {id} was not found.");
            }

            return result.Data.ToResponseDto();
        }

        public async Task<CountryResponseDto> FindAsync(string countryName)
        {
            StringGuard.AgainstNullOrEmptyString(countryName, "Country name");

            var result = await _repo.GetByNameAsync(countryName);

            result.ThrowIfNotSuccess();
            result.ThrowNotFoundIfDataNull();

            return result.Data.ToResponseDto();
        }

        public async Task<IReadOnlyList<CountryResponseDto>> GetAllAsync()
        {
            var result = await _repo.GetAllAsync();
            result.ThrowIfNotSuccess();
            return result.Data.Select(c => c.ToResponseDto()).ToList();
        }

        public async Task<IReadOnlyList<PaginationResponse<CountryResponseDto>>> GetPagedAsync(PaginationRequest paginationRequest)
        {
            ArgumentNullException.ThrowIfNull(paginationRequest);
            NumericGuard.AgainstInvalidId(paginationRequest.Page);
            NumericGuard.AgainstInvalidId(paginationRequest.PageSize);

            var result = await _repo.GetPagedAsync(
                paginationRequest.Page,
                paginationRequest.PageSize);

            result.ThrowIfNotSuccess();

            IReadOnlyList<CountryResponseDto> dtoReadOnlyList = 
                result.Data
                .Select(p => p.ToResponseDto())
                .ToList();

            return new List<PaginationResponse<CountryResponseDto>>
            {
                new PaginationResponse<CountryResponseDto>
                {
                    Items = dtoReadOnlyList,
                    TotalCount = result.Data.Count,
                    Page = paginationRequest.Page,
                    PageSize = paginationRequest.PageSize
                }
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

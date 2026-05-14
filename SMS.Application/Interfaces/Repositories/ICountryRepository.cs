using SMS.Application.Common.Results;
using SMS.Domain.Entities;
using SMS.Shared.Common;

namespace SMS.Application.Interfaces.Repositories
{
    public interface ICountryRepository
    {
        public Task<OperationResult<int>> AddAsync(Country country);
        public Task<OperationResult<bool>> ExistsAsync(int countryId);
        public Task<OperationResult<bool>> ExistsAsync(string countryName);
        public Task<OperationResult<Country?>> FindAsync(int countryId);
        public Task<OperationResult<Country?>> FindByNameAsync(string name);
        public Task<OperationResult<IReadOnlyList<Country>>> GetAllAsync();
        public Task<OperationResult<PaginationResponse<Country>>> GetPagedAsync(PaginationRequest request);
        public Task<OperationResult<bool>> UpdateAsync(Country country);
        public Task<OperationResult<bool>> DeleteAsync(int countryId);
    }
}

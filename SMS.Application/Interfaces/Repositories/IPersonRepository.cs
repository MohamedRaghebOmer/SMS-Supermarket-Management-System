using SMS.Application.Common.Results;
using SMS.Domain.Entities;
using SMS.Shared.Common;
using SMS.Shared.Enums;

namespace SMS.Application.Interfaces.Repositories
{
    public interface IPersonRepository
    {
        Task<OperationResult<int>> AddAsync(Person person);
        Task<OperationResult<Person?>> FindByIdAsync(int personId);
        Task<OperationResult<Person?>> FindByNationalNoAsync(string nationalNo);
        Task<OperationResult<PaginationResponse<Person>>> GetPagedAsync(PaginationRequest paginationRequest);
        Task<OperationResult<PaginationResponse<Person>>> GetByGenderAsync(Gender gender, PaginationRequest paginationRequest);
        Task<OperationResult<Person?>> FindByEmailAsync(string email);
        Task<OperationResult<Guid?>> GetImageAsync(int personId);
        Task<OperationResult<PaginationResponse<Person>>> GetByNationalityCountryIdAsync(int countryId, PaginationRequest paginationRequest);
        Task<OperationResult<bool>> ExistsByIdAsync(int personId);
        Task<OperationResult<bool>> ExistsByNationalNoAsync(string nationalNo);
        Task<OperationResult<bool>> ExistsByEmailAsync(string email);
        Task<OperationResult<bool>> SetImageAsync(int personId, Guid? newImageFileName);
        Task<OperationResult<bool>> UpdateAsync(Person person);
        Task<OperationResult<bool>> DeleteAsync(int personId);
        Task<OperationResult<bool>> DeleteAsync(string nationalNo);
    }
}

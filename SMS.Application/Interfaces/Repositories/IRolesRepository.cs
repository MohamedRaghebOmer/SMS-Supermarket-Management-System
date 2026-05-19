using SMS.Application.Common.Results;

namespace SMS.Application.Interfaces.Repositories
{
    public interface IRolesRepository
    {
        Task<OperationResult<string?>> FindRoleNameByIdAsync(int roleId);
    }
}

using SMS.Application.Common.Results;
using SMS.Domain.Entities;
using SMS.Shared.Common;

namespace SMS.Application.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<OperationResult<int>> RegisterAsync(User user);
        Task<OperationResult<User?>> FindByIdAsync(int userId);
        Task<OperationResult<User?>> FindByUsernameAsync(string username);
        Task<OperationResult<User?>> FindByPersonIdAsync(int personId);
        Task<OperationResult<User?>> FindByEmailAsync(string email);
        /// <summary>
        /// Gets the user identifier for the specified username.
        /// </summary>
        Task<OperationResult<int>> GetUserIdByUsernameAsync(string username);
        /// <summary>
        /// Gets the user identifier for the specified email address.
        /// </summary>
        Task<OperationResult<int>> GetUserIdByEmailAsync(string email);
        /// <summary>
        /// Gets the user identifier for the specified person identifier.
        /// </summary>
        Task<OperationResult<int>> GetUserIdByPersonIdAsync(int personId);
        Task<OperationResult<bool>> ExistsByIdAsync(int userId);
        Task<OperationResult<bool>> ExistsByUsername(string username);
        Task<OperationResult<bool>> ExistsByEmail(string email);
        Task<OperationResult<bool>> IsEmailOwnedByUserAsync(string email, int userId);
        Task<OperationResult<string>> GetPasswordHashAsync(int userId);
        Task<OperationResult<PaginationResponse<User>>> GetByRoleId(int roleId, PaginationRequest paginationRequest);
        Task<OperationResult<PaginationResponse<User>>> GetPagedAsync(PaginationRequest paginationRequest);
        Task<OperationResult<PaginationResponse<User>>> GetActiveUsers(PaginationRequest paginationRequest);
        Task<OperationResult<bool>> Login(string username, string password);
        Task<OperationResult<bool>> ChangePassword(int userId, string newPasswordHash);
        Task<OperationResult<bool>> ActivateAsync(int userId);
        Task<OperationResult<bool>> DeactivateAsync(int userId);
        Task<OperationResult<bool>> UpdateAsync(User user);
        Task<OperationResult<bool>> DeleteAsync(int userId);
    }
}

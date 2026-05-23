using SMS.Contracts.Requests.Users;
using SMS.Contracts.Responses;
using SMS.Shared.Common;

namespace SMS.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<int> RegisterAsync(CreateUserDto createDto);
        Task<UserResponseDto> GetByIdAsync(int userId);
        Task<UserResponseDto> GetByUsernameAsync(string username);
        Task<UserResponseDto> GetByPersonIdAsync(int personId);
        Task<UserResponseDto> GetByEmailAsync(string email);
        Task<bool> ExistsByIdAsync(int userId);
        Task<bool> ExistsByUsernameAsync(string username);
        Task<bool> ExistsByEmailAsync(string email);
        Task<bool> IsEmailOwnedByUserAsync(string email, int userId);
        Task<PaginationResponse<UserResponseDto>> GetByRoleIdAsync(int roleId,
            PaginationRequest paginationRequest);
        Task<PaginationResponse<UserResponseDto>> GetPagedAsync(
            PaginationRequest paginationRequest);
        Task<PaginationResponse<UserResponseDto>> GetActiveUsersAsync(
            PaginationRequest paginationRequest);
        Task<bool> ChangePasswordAsync(int userId, ChangePasswordDto dto);
        Task<bool> ActivateAsync(int userId);
        Task<bool> DeactivateAsync(int userId);
        Task<bool> UpdateAsync(int userId, UpdateUserDto updateDto);
        Task<bool> DeleteAsync(int userId);
    }
}

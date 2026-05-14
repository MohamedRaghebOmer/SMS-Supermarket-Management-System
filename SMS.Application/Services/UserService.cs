using SMS.Application.Common.Results;
using SMS.Application.Interfaces.Helpers;
using SMS.Application.Interfaces.Repositories;
using SMS.Application.Interfaces.Services;
using SMS.Application.Mapping;
using SMS.Contracts.Requests.Users;
using SMS.Contracts.Responses;
using SMS.Domain.Entities;
using SMS.Shared.Common;
using SMS.Shared.Guards;

namespace SMS.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repo;
        private readonly IStringHelper _stringHelper;

        public UserService(IUserRepository repo, IStringHelper stringHelper)
        {
            _repo = repo;
            _stringHelper = stringHelper;
        }


        public async Task<int> RegisterAsync(CreateUserDto createDto)
        {
            ArgumentNullException.ThrowIfNull(createDto);
            NumericGuard.AgainstInvalidId(createDto.PersonId);
            StringGuard.AgainstNullOrEmpty(createDto.Username, nameof(createDto.Username));
            StringGuard.AgainstNullOrEmpty(createDto.Password, nameof(createDto.Password));
            NumericGuard.AgainstInvalidId(createDto.RoleId);

            var result = await _repo.RegisterAsync(createDto.ToEntity());
            result.ThrowIfNotSuccess();

            return result.Data;
        }

        public async Task<UserResponseDto> GetByIdAsync(int userId)
        {
            NumericGuard.AgainstInvalidId(userId);

            var result = await _repo.FindByIdAsync(userId);
            result.ThrowIfNotSuccess();
            result.ThrowNotFoundIfDataNull();

            return result.Data.ToDto();
        }

        public async Task<UserResponseDto> GetByUsernameAsync(string username)
        {
            StringGuard.AgainstNullOrEmpty(username, nameof(username));

            var result = await _repo.FindByUsernameAsync(username);
            result.ThrowIfNotSuccess();
            result.ThrowNotFoundIfDataNull();

            return result.Data.ToDto();
        }

        public async Task<UserResponseDto> GetByPersonIdAsync(int personId)
        {
            NumericGuard.AgainstInvalidId(personId);

            var result = await _repo.FindByPersonIdAsync(personId);
            result.ThrowIfNotSuccess();
            result.ThrowNotFoundIfDataNull();

            return result.Data.ToDto();
        }

        public async Task<UserResponseDto> GetByEmailAsync(string email)
        {
            StringGuard.AgainstNullOrEmpty(email, nameof(email));

            var result = await _repo.FindByEmailAsync(email);
            result.ThrowIfNotSuccess();
            result.ThrowNotFoundIfDataNull();

            return result.Data.ToDto();
        }

        public async Task<bool> ExistsByIdAsync(int userId)
        {
            NumericGuard.AgainstInvalidId(userId);

            var result = await _repo.ExistsById(userId);
            result.ThrowIfNotSuccess();

            return result.Data;
        }

        public async Task<bool> ExistsByUsernameAsync(string username)
        {
            StringGuard.AgainstNullOrEmpty(username, nameof(username));

            var result = await _repo.ExistsByUsername(username);
            result.ThrowIfNotSuccess();

            return result.Data;
        }

        public async Task<bool> ExistsByEmailAsync(string email)
        {
            StringGuard.AgainstNullOrEmpty(email, nameof(email));

            var result = await _repo.ExistsByEmail(email);
            result.ThrowIfNotSuccess();

            return result.Data;
        }

        public async Task<bool> IsEmailOwnedByUserAsync(string email, int userId)
        {
            StringGuard.AgainstNullOrEmpty(email, nameof(email));
            NumericGuard.AgainstInvalidId(userId);

            var result = await _repo.IsEmailOwnedByUserAsync(email, userId);
            result.ThrowIfNotSuccess();

            return result.Data;
        }

        public async Task<PaginationResponse<UserResponseDto>> GetByRoleIdAsync(int roleId, PaginationRequest paginationRequest)
        {
            NumericGuard.AgainstInvalidId(roleId);
            ValidatePagination(paginationRequest);

            var result = await _repo.GetByRoleId(roleId, paginationRequest);
            result.ThrowIfNotSuccess();

            return BuildPagedResponse(result, paginationRequest);
        }

        public async Task<PaginationResponse<UserResponseDto>> GetPagedAsync(PaginationRequest paginationRequest)
        {
            ValidatePagination(paginationRequest);

            var result = await _repo.GetPagedAsync(paginationRequest);
            result.ThrowIfNotSuccess();

            return BuildPagedResponse(result, paginationRequest);
        }

        public async Task<PaginationResponse<UserResponseDto>> GetActiveUsersAsync(PaginationRequest paginationRequest)
        {
            ValidatePagination(paginationRequest);

            var result = await _repo.GetActiveUsers(paginationRequest);
            result.ThrowIfNotSuccess();

            return BuildPagedResponse(result, paginationRequest);
        }

        public async Task<bool> ChangePasswordAsync(int userId, ChangePasswordDto dto)
        {
            NumericGuard.AgainstInvalidId(userId);
            StringGuard.AgainstNullOrEmpty(dto.OldPassword, nameof(dto.OldPassword));
            StringGuard.AgainstNullOrEmpty(dto.NewPassword, nameof(dto.NewPassword));

            if (dto.OldPassword == dto.NewPassword)
            {
                throw new ArgumentException("New password must be different from the old password.");
            }

            OperationResult<string> passwordHash = await _repo.GetPasswordHashAsync(userId);

            if (!passwordHash.IsSuccess || passwordHash.Data == null)
            {
                // This exception will be caught by the global exception handler
                // and converted to a 500 Internal Server Error response
                throw new InvalidOperationException("Failed to retrieve password hash.");
            }

            if (!_stringHelper.Verify(dto.OldPassword, passwordHash.Data))
            {
                throw new ArgumentException("Old password is incorrect.");
            }

            var newPasswordHash = _stringHelper.Hash(dto.NewPassword);

            var result = await _repo.ChangePassword(userId, newPasswordHash);
            result.ThrowIfNotSuccess();

            return result.Data;
        }

        public async Task<bool> ActivateAsync(int userId)
        {
            NumericGuard.AgainstInvalidId(userId);

            var result = await _repo.ActivateAsync(userId);
            result.ThrowIfNotSuccess();

            return result.Data;
        }

        public async Task<bool> DeactivateAsync(int userId)
        {
            NumericGuard.AgainstInvalidId(userId);

            var result = await _repo.DeactivateAsync(userId);
            result.ThrowIfNotSuccess();

            return result.Data;
        }

        public async Task<bool> UpdateAsync(UpdateUserDto updateDto)
        {
            ArgumentNullException.ThrowIfNull(updateDto);
            NumericGuard.AgainstInvalidId(updateDto.PersonId);
            StringGuard.AgainstNullOrEmpty(updateDto.Username, nameof(updateDto.Username));
            StringGuard.AgainstNullOrEmpty(updateDto.Password, nameof(updateDto.Password));
            NumericGuard.AgainstInvalidId(updateDto.RoleId);

            var result = await _repo.UpdateAsync(updateDto.ToEntity());
            result.ThrowIfNotSuccess();

            return result.Data;
        }

        public async Task<bool> DeleteAsync(int userId)
        {
            NumericGuard.AgainstInvalidId(userId);

            var result = await _repo.DeleteAsync(userId);
            result.ThrowIfNotSuccess();

            return result.Data;
        }


        private static void ValidatePagination(PaginationRequest paginationRequest)
        {
            ArgumentNullException.ThrowIfNull(paginationRequest);
            NumericGuard.AgainstInvalidId(paginationRequest.Page);
            NumericGuard.AgainstInvalidId(paginationRequest.PageSize);
        }

        private static PaginationResponse<UserResponseDto> BuildPagedResponse(
            OperationResult<PaginationResponse<User>> result,
            PaginationRequest paginationRequest)
        {
            return new PaginationResponse<UserResponseDto>
            {
                Items = result.Data.Items.Select(user => user.ToDto()).ToList(),
                TotalCount = result.Data.TotalCount,
                Page = paginationRequest.Page,
                PageSize = paginationRequest.PageSize
            };
        }
    }
}

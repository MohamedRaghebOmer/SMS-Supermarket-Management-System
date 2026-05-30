using SMS.Application.Common.Results;
using SMS.Application.Exceptions;
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
        private readonly IValidationHelper _validationHelper;

        public UserService(IUserRepository repo, IStringHelper stringHelper, IValidationHelper validationHelper)
        {
            _repo = repo;
            _stringHelper = stringHelper;
            _validationHelper = validationHelper;
        }


        public async Task<int> RegisterAsync(CreateUserDto createDto)
        {
            ArgumentNullException.ThrowIfNull(createDto);
            NumericGuard.AgainstInvalidId(createDto.PersonId);
            StringGuard.AgainstNullOrWhiteSpace(createDto.Username, nameof(createDto.Username));
            StringGuard.AgainstNullOrWhiteSpace(createDto.Password, nameof(createDto.Password));
            NumericGuard.AgainstInvalidId(createDto.RoleId);

            if (createDto.Password.Length < 8)
            {
                throw new ArgumentException("New password must be at least 8 characters long.");
            }

            var hashedDto = createDto with
            {
                Password = _stringHelper.Hash(createDto.Password)
            };

            var result = await _repo.RegisterAsync(hashedDto.ToEntity());
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
            StringGuard.AgainstNullOrWhiteSpace(username, nameof(username));

            var result = await _repo.FindByUsernameAsync(username);
            result.ThrowIfNotSuccess();
            result.ThrowNotFoundIfDataNull();

            return result.Data.ToDto();
        }

        /// <summary>
        /// Gets the user identifier for the specified username.
        /// </summary>
        public async Task<int> GetUserIdByUsernameAsync(string username)
        {
            StringGuard.AgainstNullOrWhiteSpace(username, nameof(username));

            var result = await _repo.GetUserIdByUsernameAsync(username);
            result.ThrowIfNotSuccess();

            return result.Data;
        }

        /// <summary>
        /// Gets the user identifier for the specified email address.
        /// </summary>
        public async Task<int> GetUserIdByEmailAsync(string email)
        {
            _validationHelper.ValidateEmail(email, nameof(email));

            var result = await _repo.GetUserIdByEmailAsync(email);
            result.ThrowIfNotSuccess();

            return result.Data;
        }

        /// <summary>
        /// Gets the user identifier for the specified person identifier.
        /// </summary>
        public async Task<int> GetUserIdByPersonIdAsync(int personId)
        {
            NumericGuard.AgainstInvalidId(personId);

            var result = await _repo.GetUserIdByPersonIdAsync(personId);
            result.ThrowIfNotSuccess();

            return result.Data;
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
            _validationHelper.ValidateEmail(email, nameof(email));

            var result = await _repo.FindByEmailAsync(email);
            result.ThrowIfNotSuccess();
            result.ThrowNotFoundIfDataNull();

            return result.Data.ToDto();
        }

        public async Task<bool> ExistsByIdAsync(int userId)
        {
            NumericGuard.AgainstInvalidId(userId);

            var result = await _repo.ExistsByIdAsync(userId);
            result.ThrowIfNotSuccess();

            return result.Data;
        }

        public async Task<bool> ExistsByUsernameAsync(string username)
        {
            StringGuard.AgainstNullOrWhiteSpace(username, nameof(username));

            var result = await _repo.ExistsByUsername(username);
            result.ThrowIfNotSuccess();

            return result.Data;
        }

        public async Task<bool> ExistsByEmailAsync(string email)
        {
            _validationHelper.ValidateEmail(email, nameof(email));

            var result = await _repo.ExistsByEmail(email);
            result.ThrowIfNotSuccess();

            return result.Data;
        }

        /// <summary>
        /// Determines whether the specified user is active.
        /// </summary>
        public async Task<bool> IsActiveAsync(int userId)
        {
            NumericGuard.AgainstInvalidId(userId);

            var result = await _repo.IsActive(userId);
            result.ThrowIfNotSuccess();

            return result.Data;
        }

        public async Task<bool> IsEmailOwnedByUserAsync(string email, int userId)
        {
            _validationHelper.ValidateEmail(email, nameof(email));
            NumericGuard.AgainstInvalidId(userId);

            var result = await _repo.IsEmailOwnedByUserAsync(email, userId);
            result.ThrowIfNotSuccess();

            return result.Data;
        }

        public async Task<PaginationResponse<UserResponseDto>> GetByRoleIdAsync(int roleId, PaginationRequest paginationRequest)
        {
            NumericGuard.AgainstInvalidId(roleId);
            _validationHelper.ValidatePagination(paginationRequest);

            var result = await _repo.GetByRoleId(roleId, paginationRequest);
            result.ThrowIfNotSuccess();

            return BuildPagedResponse(result, paginationRequest);
        }

        public async Task<PaginationResponse<UserResponseDto>> GetPagedAsync(PaginationRequest paginationRequest)
        {
            _validationHelper.ValidatePagination(paginationRequest);

            var result = await _repo.GetPagedAsync(paginationRequest);
            result.ThrowIfNotSuccess();

            return BuildPagedResponse(result, paginationRequest);
        }

        public async Task<PaginationResponse<UserResponseDto>> GetActiveUsersAsync(PaginationRequest paginationRequest)
        {
            _validationHelper.ValidatePagination(paginationRequest);

            var result = await _repo.GetActiveUsers(paginationRequest);
            result.ThrowIfNotSuccess();

            return BuildPagedResponse(result, paginationRequest);
        }

        public async Task<bool> ChangePasswordAsync(int userId, ChangePasswordDto dto)
        {
            NumericGuard.AgainstInvalidId(userId);
            StringGuard.AgainstNullOrWhiteSpace(dto.OldPassword, nameof(dto.OldPassword));
            StringGuard.AgainstNullOrWhiteSpace(dto.NewPassword, nameof(dto.NewPassword));

            if (string.IsNullOrWhiteSpace(dto.ConfirmNewPassword)
                || dto.NewPassword != dto.ConfirmNewPassword)
            {
                throw new ArgumentException("New password and confirmation do not match.");
            }

            if (dto.NewPassword.Length < 8)
            {
                throw new ArgumentException("New password must be at least 8 characters long.");
            }

            if (dto.OldPassword == dto.NewPassword)
            {
                throw new ArgumentException("New password must be different from the old password.");
            }

            OperationResult<string> passwordHash = await _repo.GetPasswordHashAsync(userId);

            if (!passwordHash.IsSuccess || passwordHash.Data == null)
            {
                // This exception will be caught by the global exception handler
                // and converted to a 500 Internal Server Error response
                throw new NotFoundException("User not found.");
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

        public async Task<bool> UpdateAsync(int userId, UpdateUserDto updateDto)
        {
            NumericGuard.AgainstInvalidId(userId);
            ArgumentNullException.ThrowIfNull(updateDto);
            NumericGuard.AgainstInvalidId(updateDto.PersonId);
            StringGuard.AgainstNullOrWhiteSpace(updateDto.Username, nameof(updateDto.Username));
            StringGuard.AgainstNullOrWhiteSpace(updateDto.Password, nameof(updateDto.Password));
            NumericGuard.AgainstInvalidId(updateDto.RoleId);

            if (updateDto.Password.Length < 8)
            {
                throw new ArgumentException("New password must be at least 8 characters long.");
            }

            var entity = updateDto.ToEntity();
            entity.UserId = userId;
            entity.PasswordHash = _stringHelper.Hash(updateDto.Password);

            var result = await _repo.UpdateAsync(entity);
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

using SMS.Application.Common.Results;
using SMS.Application.Exceptions;
using SMS.Application.Interfaces.Helpers;
using SMS.Application.Interfaces.Repositories;
using SMS.Application.Interfaces.Services;
using SMS.Application.Mapping;
using SMS.Contracts.Requests.Roles;
using SMS.Contracts.Responses;
using SMS.Domain.Entities;
using SMS.Shared.Common;
using SMS.Shared.Guards;

namespace SMS.Application.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _repo;
        private readonly IValidationHelper _validationHelper;

        public RoleService(IRoleRepository repo, IValidationHelper validationHelper)
        {
            _repo = repo;
            _validationHelper = validationHelper;
        }

        public async Task<int> AddAsync(CreateRoleRequestDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);

            var result = await _repo.AddAsync(dto.ToEntity());
            result.ThrowIfNotSuccess();

            return result.Data;
        }

        public async Task<RoleResponseDto> GetByIdAsync(int roleId)
        {
            NumericGuard.AgainstInvalidId(roleId);

            var result = await _repo.FindByIdAsync(roleId);
            result.ThrowIfNotSuccess();
            result.ThrowNotFoundIfDataNull();

            return result.Data!.ToDto();
        }

        public async Task<RoleResponseDto> GetByNameAsync(string roleName)
        {
            StringGuard.AgainstNullOrWhiteSpace(roleName, nameof(roleName));

            var result = await _repo.FindByNameAsync(roleName);
            result.ThrowIfNotSuccess();
            result.ThrowNotFoundIfDataNull();

            return result.Data!.ToDto();
        }

        public async Task<PaginationResponse<RoleResponseDto>> GetPagedAsync(PaginationRequest request)
        {
            _validationHelper.ValidatePagination(request);

            var result = await _repo.GetPagedAsync(request);
            result.ThrowIfNotSuccess();

            return BuildPaginationResponse(result);
        }

        public async Task<PaginationResponse<RoleResponseDto>> GetPagedByIsActiveAsync(PaginationRequest request, bool isActive)
        {
            _validationHelper.ValidatePagination(request);

            var result = await _repo.GetPagedByIsActiveAsync(request, isActive);
            result.ThrowIfNotSuccess();

            return BuildPaginationResponse(result);
        }

        public async Task<PaginationResponse<RoleResponseDto>> GetPagedByCreatedAtRangeAsync(PaginationRequest request, DateTime from, DateTime to)
        {
            _validationHelper.ValidatePagination(request);
            SMS.Shared.Guards.DateGuard.AgainstInvalidDateRange(from, to, nameof(from), nameof(to));

            var result = await _repo.GetPagedByCreatedAtRangeAsync(request, from, to);
            result.ThrowIfNotSuccess();

            return BuildPaginationResponse(result);
        }

        public async Task<string> GetRoleNameByIdAsync(int roleId)
        {
            NumericGuard.AgainstInvalidId(roleId);

            if (roleId == 1)
                return "Admin";

            var result = await _repo.FindRoleNameByIdAsync(roleId);
            result.ThrowIfNotSuccess();

            return result.Data!;
        }

        public async Task<bool> IsActive(int roleId)
        {
            NumericGuard.AgainstInvalidId(roleId);

            if (roleId == 1) // Admin role is always active
                return true;

            var result = await _repo.IsActive(roleId);
            result.ThrowIfNotSuccess();

            return result.Data;
        }

        public async Task<bool> UpdateAsync(int roleId, UpdateRoleRequestDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);
            NumericGuard.AgainstInvalidId(roleId);

            if (roleId == 1)
                throw new ValidationException("Admin role cannot be updated.");

            var result = await _repo.UpdateAsync(dto.ToEntity(roleId));
            result.ThrowIfNotSuccess();

            return result.Data;
        }

        public async Task<bool> ActivateAsync(int roleId)
        {
            NumericGuard.AgainstInvalidId(roleId);

            if (roleId == 1)
                throw new ValidationException("Admin role is already active.");

            var result = await _repo.ActivateAsync(roleId);
            result.ThrowIfNotSuccess();

            return result.Data;
        }

        public async Task<bool> DeactivateAsync(int roleId)
        {
            NumericGuard.AgainstInvalidId(roleId);

            if (roleId == 1)
                throw new ValidationException("Admin role cannot be deactivated.");

            var result = await _repo.DeactivateAsync(roleId);
            result.ThrowIfNotSuccess();

            return result.Data;
        }

        private PaginationResponse<RoleResponseDto> BuildPaginationResponse(OperationResult<PaginationResponse<Role>> result)
        {
            return new PaginationResponse<RoleResponseDto>
            {
                Items = result.Data!.Items.Select(r => r.ToDto()).ToList(),
                TotalCount = result.Data.TotalCount,
                Page = result.Data.Page,
                PageSize = result.Data.PageSize
            };
        }
    }
}

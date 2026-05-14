using Microsoft.AspNetCore.Authorization;
using SMS.API.Authorization.Requirements;
using SMS.Application.Interfaces.Services;

namespace SMS.API.Authorization.Handlers
{
    public sealed class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly IRoleEntityPermissionService _permissionService;

        public PermissionAuthorizationHandler(IRoleEntityPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            PermissionRequirement requirement)
        {
            var roleIdClaim = context.User.FindFirst("RoleId")?.Value;

            if (string.IsNullOrWhiteSpace(roleIdClaim) ||
                !int.TryParse(roleIdClaim, out int roleId))
            {
                return;
            }

            if (roleId == 1) // Admin Role
            {
                context.Succeed(requirement);
                return;
            }

            bool hasPermission = await _permissionService.HasPermissionAsync(roleId, requirement.Entity, requirement.Action);

            if (hasPermission)
            {
                context.Succeed(requirement);
            }
        }
    }
}
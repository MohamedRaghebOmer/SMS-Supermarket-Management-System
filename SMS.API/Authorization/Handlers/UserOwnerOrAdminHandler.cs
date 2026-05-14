using Microsoft.AspNetCore.Authorization;
using SMS.API.Authorization.Requirements;
using System.Security.Claims;

namespace SMS.API.Authorization.Handlers
{
    public class UserOwnerOrAdminHandler
        : AuthorizationHandler<UserOwnerOrAdminRequirement, int>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            UserOwnerOrAdminRequirement requirement,
            int resourceUserId)
        {
            string? userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId is null)
                return Task.CompletedTask;

            bool isAdmin = context.User.IsInRole("Admin");

            bool isOwner = int.TryParse(userId, out int currentUserId)
                           && currentUserId == resourceUserId;

            if (isAdmin || isOwner)
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}

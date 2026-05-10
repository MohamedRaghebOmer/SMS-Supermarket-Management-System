using Microsoft.AspNetCore.Authorization;
using SMS.Shared.Enums;

namespace SMS.API.Authorization
{
    public sealed class PermissionRequirement : IAuthorizationRequirement
    {
        public SystemEntity Entity { get; }
        public PermissionAction Action { get; }

        public PermissionRequirement(SystemEntity entity, PermissionAction action)
        {
            Entity = entity;
            Action = action;
        }
    }
}
using Microsoft.AspNetCore.Authorization;
using SMS.Shared.Enums;

namespace SMS.API.CustomAttributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class RequirePermissionAttribute : AuthorizeAttribute
    {
        public PermissionAction Action { get; }
        public SystemEntity Entity { get; }

        public RequirePermissionAttribute(PermissionAction action, SystemEntity entity)
        {
            Policy = $"Permission:{entity}:{action}";
            Action = action;
            Entity = entity;
        }
    }
}

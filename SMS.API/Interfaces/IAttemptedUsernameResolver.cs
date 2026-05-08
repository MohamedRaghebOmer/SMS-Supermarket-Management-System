using SMS.Shared.Enums;

namespace SMS.API.Interfaces
{
    public interface IAttemptedUsernameResolver
    {
        Task<string?> ResolveAsync(HttpContext context, AuditActionType actionType);
    }
}

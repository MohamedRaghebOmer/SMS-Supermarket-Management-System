using SMS.Shared.Enums;

namespace SMS.API.Interfaces
{
    public interface IAuditActionTypeResolver
    {
        AuditActionType Resolve(HttpContext context);
    }
}

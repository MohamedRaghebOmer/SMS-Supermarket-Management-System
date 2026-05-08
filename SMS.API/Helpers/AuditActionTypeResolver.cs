using SMS.API.CustomAttributes;
using SMS.API.Interfaces;
using SMS.Shared.Enums;

namespace SMS.API.Helpers
{
    public class AuditActionTypeResolver : IAuditActionTypeResolver
    {
        public AuditActionType Resolve(HttpContext context)
        {
            var endpoint = context.GetEndpoint();
            if (endpoint is null)
                return AuditActionType.Unknown;

            var explicitAttr = endpoint.Metadata.GetMetadata<AuditActionTypeAttribute>();
            if (explicitAttr is not null)
                return explicitAttr.ActionType;

            var method = context.Request.Method;

            if (HttpMethods.IsPost(method))
                return AuditActionType.Insert;

            if (HttpMethods.IsGet(method))
                return AuditActionType.Read;

            if (HttpMethods.IsPut(method) || HttpMethods.IsPatch(method))
                return AuditActionType.Update;

            if (HttpMethods.IsDelete(method))
                return AuditActionType.Delete;

            return AuditActionType.Unknown;
        }
    }
}

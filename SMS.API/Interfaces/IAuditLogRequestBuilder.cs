using SMS.Contracts.Requests.AuditLogs;

namespace SMS.API.Interfaces
{
    public interface IAuditLogRequestBuilder
    {
        Task<AuditLogRequestDto> BuildAsync(
            HttpContext context,
            string responseBody,
            int duration);
    }
}

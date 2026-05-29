using SMS.Shared.Enums;

namespace SMS.Contracts.Requests.AuditLogs
{
    /// <summary>
    ///  For internal use only, not intended for external clients.
    ///  This DTO is used to encapsulate the data required for creating an audit log entry.
    /// </summary>
    public class AuditLogRequestDto
    {
        public int? UserId { get; set; }
        public string? AttemptedLoginIdentifier { get; set; } = null;
        public Guid? CorrelationId { get; set; }
        public AuditActionType ActionType { get; set; }
        public required string Endpoint { get; set; }
        public string? RequestBody { get; set; } = null;
        public string? ResponseBody { get; set; } = null;
        public string? UserAgent { get; set; } = null;
        public int HttpStatusCode { get; set; }
        public bool IsSuccess => (HttpStatusCode >= 200 && HttpStatusCode < 300);
        public int Duration { get; set; }
        public required string IpAddress { get; set; }
        public string? Details { get; set; } = null;
    }
}

using SMS.Shared.Enums;

namespace SMS.Contracts.Requests.AuditLogs
{
    /// <summary>
    ///  For internal use only, not intended for external clients.
    ///  This DTO is used to encapsulate the data required for creating an audit log entry.
    /// </summary>
    public sealed record AuditLogRequestDto
    {
        public int? UserId { get; init; }
        public string? AttemptedLoginIdentifier { get; init; } = null;
        public Guid? CorrelationId { get; init; }
        public AuditActionType ActionType { get; init; }
        public required string Endpoint { get; init; }
        public string? RequestBody { get; init; } = null;
        public string? ResponseBody { get; init; } = null;
        public string? UserAgent { get; init; } = null;
        public int HttpStatusCode { get; init; }
        public bool IsSuccess => (HttpStatusCode >= 200 && HttpStatusCode < 300);
        public int Duration { get; init; }
        public required string IpAddress { get; init; }
        public string? Details { get; init; } = null;
    }
}

using SMS.Shared.Enums;

namespace SMS.Contracts.Responses
{
    public sealed record AuditLogResponseDto
    {
        public long AuditLogId { get; init; }
        public int? UserId { get; init; }
        public string? AttemptedLoginIdentifier { get; init; }
        public Guid? CorrelationId { get; init; }
        public AuditActionType ActionType { get; init; }
        public required string Endpoint { get; init; }
        public string? RequestBody { get; init; }
        public string? ResponseBody { get; init; }
        public string? UserAgent { get; init; }
        public int HttpStatusCode { get; init; }
        public bool IsSuccess => (HttpStatusCode >= 200 && HttpStatusCode < 300);
        public int Duration { get; init; }
        public required string IpAddress { get; init; }
        public string? Details { get; init; }
        public DateTime CreatedAt { get; init; }
    }
}

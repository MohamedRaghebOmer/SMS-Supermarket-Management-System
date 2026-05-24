using SMS.Shared.Enums;

namespace SMS.Contracts.Responses
{
    public class AuditLogResponseDto
    {
        public long AuditLogId { get; set; }
        public int? UserId { get; set; }
        public string? AttemptedLoginIdentifier { get; set; }
        public Guid? CorrelationId { get; set; }
        public AuditActionType ActionType { get; set; }
        public required string Endpoint { get; set; }
        public string? RequestBody { get; set; }
        public string? ResponseBody { get; set; }
        public string? UserAgent { get; set; }
        public int HttpStatusCode { get; set; }
        public bool IsSuccess => (HttpStatusCode >= 200 && HttpStatusCode < 300);
        public int Duration { get; set; }
        public required string IpAddress { get; set; }
        public string? Details { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

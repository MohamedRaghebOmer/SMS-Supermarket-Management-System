using SMS.Shared.Enums;
using System.Net;

namespace SMS.Contracts.Responses
{
    public class AuditLogResponseDto
    {
        public long AuditLogId { get; set; }
        public int? UserId { get; set; }
        public string? AttemptedLoginIdentifier { get; set; }
        public Guid CorrelationId { get; set; }
        public AuditActionType ActionType { get; set; }
        public required string Endpoint { get; set; }
        public string? RequestBody { get; set; }
        public string? ResponseBody { get; set; }
        public string? UserAgent { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public bool IsSuccess { get; set; }
        public int Duration { get; set; }
        public required string IpAddress { get; set; }
        public string? Details { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

using SMS.Shared.Enums;

namespace SMS.Contracts.Responses
{
    public sealed record ApplicationLogResponseDto
    {
        public int ApplicationLogId { get; init; }
        public LogLevel LogLevel { get; init; }
        public long? AuditLogId { get; init; }
        public string Message { get; init; } = string.Empty;
        public Exception? Exception { get; init; }
        public string? StackTrace { get; init; }
    }
}

namespace SMS.Contracts.Responses
{
    public class ApplicationLogResponseDto
    {
        public int ApplicationLogId { get; set; }
        public long? AuditLogId { get; set; }
        public string Message { get; set; } = string.Empty;
        public Exception? Exception { get; set; }
        public string? StackTrace { get; set; }
    }
}

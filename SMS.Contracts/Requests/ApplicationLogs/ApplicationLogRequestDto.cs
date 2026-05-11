namespace SMS.Contracts.Requests.ApplicationLogs
{
    public class ApplicationLogRequestDto
    {
        public long? AuditLogId { get; set; }
        public required string Message { get; set; }
        public Exception? Exception { get; set; }
        public string? StackTrace { get; set; }
    }
}

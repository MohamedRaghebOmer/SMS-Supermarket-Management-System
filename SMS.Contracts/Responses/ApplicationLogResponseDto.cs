namespace SMS.Contracts.Responses
{
    public class ApplicationLogResponseDto
    {
        public int ApplicationLogId { get; set; }
        public int? AuditLogId { get; set; }
        public string Message { get; set; }
        public Exception? Exception { get; set; }
        public string? StackTrace { get; set; }
    }
}

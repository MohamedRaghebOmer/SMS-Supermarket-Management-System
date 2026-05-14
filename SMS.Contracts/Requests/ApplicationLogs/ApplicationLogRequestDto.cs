using SMS.Shared.Enums;

namespace SMS.Contracts.Requests.ApplicationLogs
{
    /// <summary>
    /// For internal use only. Represents the data required to create a new application log entry.
    /// This object does not leave the server side and is not exposed to clients.
    /// It is used to encapsulate the details of an application log when adding it to the database.
    /// </summary>
    public class ApplicationLogRequestDto
    {
        public long? AuditLogId { get; set; }
        public LogLevel LogLevel { get; set; }
        public required string Message { get; set; }
        public Exception? Exception { get; set; }
        public string? StackTrace { get; set; }
    }
}

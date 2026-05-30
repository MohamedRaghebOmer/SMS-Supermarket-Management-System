using SMS.Shared.Enums;

namespace SMS.Contracts.Requests.ApplicationLogs
{
    /// <summary>
    /// For internal use only. Represents the data required to create a new application log entry.
    /// This object does not leave the server side and is not exposed to clients.
    /// It is used to encapsulate the details of an application log when adding it to the database.
    /// </summary>
    public sealed record ApplicationLogRequestDto
    {
        public long? AuditLogId { get; init; }
        public LogLevel LogLevel { get; init; }
        public required string Message { get; init; }
        public Exception? Exception { get; init; }
        public string? StackTrace { get; init; }
    }
}

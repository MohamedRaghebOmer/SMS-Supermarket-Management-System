using SMS.Shared.Guards;

namespace SMS.Domain.Entities
{
    public class ApplicationLog
    {
        private int _applicationLogId;
        private long? _auditLogId;
        private string _message = string.Empty;


        public int ApplicationLogId
        {
            get => _applicationLogId;

            private set
            {
                NumericGuard.AgainstInvalidId(value);
                _applicationLogId = value;
            }
        }

        public long? AuditLogId
        {
            get => _auditLogId;

            set
            {
                NumericGuard.AgainstInvalidId(value);
                _auditLogId = value;
            }
        }

        public string Message
        {
            get => _message;
            set
            {
                StringGuard.AgainstNullOrEmptyString(value, "Message");
                _message = value;
            }
        }

        public Exception? Exception { get; set; }
        public string? StackTrace { get; set; }


        public ApplicationLog() { }

        public ApplicationLog(long? auditLogId, string message, Exception? exception = null, string? stackTrace = null)
        {
            AuditLogId = auditLogId;
            Message = message;
            Exception = exception;
            StackTrace = stackTrace;
        }

        public ApplicationLog(int applicationLogId, long? auditLogId, string message, Exception? exception = null, string? stackTrace = null) : this(auditLogId, message, exception, stackTrace)
        {
            ApplicationLogId = applicationLogId;
        }
    }
}

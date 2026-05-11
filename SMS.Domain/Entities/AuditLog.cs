using SMS.Shared.Enums;
using SMS.Shared.Guards;
using System.Net;

namespace SMS.Domain.Entities
{
    public class AuditLog
    {
        private long _auditLogId;
        private int? _userId;
        private Guid _correlationId;
        private string _endpoint = string.Empty;
        private int _duration;
        private string _ipAddress = string.Empty;
        private DateTime _createdAt;


        public long AuditLogId
        {
            get => _auditLogId;

            private set
            {
                NumericGuard.AgainstInvalidId(value);
                _auditLogId = value;
            }
        }

        public int? UserId
        {
            get => _userId;

            set
            {
                NumericGuard.AgainstInvalidId(value);
                _userId = value;
            }
        }

        public string? AttemptedLoginIdentifier { get; set; }

        public Guid RequestGuid
        {
            get => _correlationId;

            set
            {
                if (value == Guid.Empty)
                {
                    throw new ArgumentException("Invalid Request Guid: Guid cannot be empty.", nameof(RequestGuid));
                }

                _correlationId = value;
            }
        }

        public AuditActionType ActionType { get; set; }

        public string Endpoint
        {
            get => _endpoint;

            set
            {
                StringGuard.AgainstNullOrEmptyString(value, nameof(Endpoint));
                _endpoint = value;
            }
        }

        public string? RequestBody { get; set; }

        public string? ResponseBody { get; set; }

        public string? UserAgent { get; set; }

        public HttpStatusCode StatusCode { get; set; }

        public bool IsSuccess { get; set; }

        public int Duration
        {
            get => _duration;

            set
            {
                NumericGuard.AgainstNonPositiveNumber(value, nameof(Duration));
                _duration = value;
            }
        }

        public string IpAddress
        {
            get => _ipAddress;
            set
            {
                StringGuard.AgainstNullOrEmptyString(value, nameof(IpAddress));
                _ipAddress = value;
            }
        }

        public string? Details { get; set; } = null;

        public DateTime CreatedAt
        {
            get => _createdAt;

            set
            {
                DateGuard.AgainstFutureDate(value, nameof(CreatedAt));
                _createdAt = value;
            }
        }


        public AuditLog() { }

        public AuditLog(int? userId, string? attemptedLoginIdentifier, Guid correlationId, AuditActionType actionType, string endpoint, string? requestBody, string? responseBody, string? userAgent, HttpStatusCode statusCode, bool isSuccess, int duration, string ipAddress, string? details)
        {
            UserId = userId;
            AttemptedLoginIdentifier = attemptedLoginIdentifier;
            RequestGuid = correlationId;
            ActionType = actionType;
            Endpoint = endpoint;
            RequestBody = requestBody;
            ResponseBody = responseBody;
            UserAgent = userAgent;
            StatusCode = statusCode;
            IsSuccess = isSuccess;
            Duration = duration;
            IpAddress = ipAddress;
            Details = details;
        }

        public AuditLog(long auditLogId, int? userId, string? attemptedLoginIdentifier, Guid correlationId, AuditActionType actionType, string endpoint, string? requestBody, string? responseBody, string? userAgent, HttpStatusCode statusCode, bool isSuccess, int duration, string ipAddress, string? details, DateTime createdAt) : this(userId, attemptedLoginIdentifier, correlationId, actionType, endpoint, requestBody, responseBody, userAgent, statusCode, isSuccess, duration, ipAddress, details)
        {
            AuditLogId = auditLogId;
        }
    }
}

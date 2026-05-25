using SMS.API.Interfaces;
using SMS.Contracts.Requests.AuditLogs;

namespace SMS.API.Helpers
{
    public class AuditLogRequestBuilder : IAuditLogRequestBuilder
    {
        private readonly IAuditActionTypeResolver _actionTypeResolver;
        private readonly IAttemptedUsernameResolver _attemptedUsernameResolver;

        public AuditLogRequestBuilder(
            IAuditActionTypeResolver actionTypeResolver,
            IAttemptedUsernameResolver attemptedUsernameResolver)
        {
            _actionTypeResolver = actionTypeResolver;
            _attemptedUsernameResolver = attemptedUsernameResolver;
        }

        public async Task<AuditLogRequestDto> BuildAsync(
            HttpContext context,
            string responseBody,
            int duration)
        {
            var userId = AuditLogHelper.GetUserId(context);
            var actionType = _actionTypeResolver.Resolve(context);
            var attemptedLoginIdentifier = await _attemptedUsernameResolver.ResolveAsync(context, actionType);
            var correlationId = AuditLogHelper.GetOrCreateCorrelationId(context);
            var endpoint = AuditLogHelper.GetEndpoint(context);
            var requestBody = await AuditLogHelper.GetRequestBodyAsync(context);
            var maskedResponseBody = AuditLogHelper.MaskSensitiveData(responseBody);
            var userAgent = AuditLogHelper.GetUserAgent(context);
            var httpStatusCode = AuditLogHelper.GetStatusCode(context);
            var durationInMilliseconds = duration;
            var ipAddress = AuditLogHelper.GetIpAddress(context);

            return new AuditLogRequestDto
            {
                UserId = userId,
                AttemptedLoginIdentifier = attemptedLoginIdentifier,
                CorrelationId = correlationId,
                ActionType = actionType,
                Endpoint = endpoint,
                RequestBody = requestBody,
                ResponseBody = maskedResponseBody,
                UserAgent = userAgent,
                HttpStatusCode = httpStatusCode,
                Duration = durationInMilliseconds,
                IpAddress = ipAddress
            };
        }
    }
}

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
            var actionType = _actionTypeResolver.Resolve(context);

            return new AuditLogRequestDto
            {
                UserId = AuditLogHelper.GetUserId(context),

                AttemptedLoginIdentifier =
                    await _attemptedUsernameResolver.ResolveAsync(
                        context,
                        actionType),

                CorrelationId =
                    AuditLogHelper.GetOrCreateCorrelationId(context),

                ActionType = actionType,

                Endpoint =
                    AuditLogHelper.GetEndpoint(context),

                RequestBody =
                    await AuditLogHelper.GetRequestBodyAsync(context),

                ResponseBody = AuditLogHelper.MaskSensitiveData(responseBody),

                UserAgent =
                    AuditLogHelper.GetUserAgent(context),

                HttpStatusCode =
                    AuditLogHelper.GetStatusCode(context),

                Duration = duration,

                IpAddress =
                    AuditLogHelper.GetIpAddress(context),
            };
        }
    }
}

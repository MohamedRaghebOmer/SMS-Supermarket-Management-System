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
                UserId = Helpers.AuditLogHelper.GetUserId(context),

                AttemptedLoginIdentifier =
                    await _attemptedUsernameResolver.ResolveAsync(
                        context,
                        actionType),

                CorrelationId =
                    Helpers.AuditLogHelper.GetOrCreateCorrelationId(context),

                ActionType = actionType,

                Endpoint =
                    Helpers.AuditLogHelper.GetEndpoint(context),

                RequestBody =
                    await Helpers.AuditLogHelper.GetRequestBodyAsync(context),

                ResponseBody = responseBody,

                UserAgent =
                    Helpers.AuditLogHelper.GetUserAgent(context),

                StatusCode =
                    Helpers.AuditLogHelper.GetStatusCode(context),

                Duration = duration,

                IpAddress =
                    Helpers.AuditLogHelper.GetIpAddress(context),
            };
        }
    }
}

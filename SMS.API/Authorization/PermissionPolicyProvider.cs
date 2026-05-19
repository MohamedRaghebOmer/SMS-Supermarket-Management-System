using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using SMS.API.Authorization.Requirements;
using SMS.Shared.Enums;

namespace SMS.API.Authorization
{
    public sealed class PermissionPolicyProvider : DefaultAuthorizationPolicyProvider
    {
        public PermissionPolicyProvider(IOptions<AuthorizationOptions> options)
            : base(options)
        {
        }

        public override async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
        {
            if (policyName.StartsWith("Permission:", StringComparison.OrdinalIgnoreCase))
            {
                string[] parts = policyName.Split(':');

                if (parts.Length == 3 &&
                    Enum.TryParse(parts[1], out SystemEntity entity) &&
                    Enum.TryParse(parts[2], out PermissionAction action))
                {
                    var policy = new AuthorizationPolicyBuilder();
                    policy.AddRequirements(new PermissionRequirement(entity, action));

                    return await Task.FromResult(policy.Build());
                }
            }

            return await base.GetPolicyAsync(policyName);
        }
    }
}
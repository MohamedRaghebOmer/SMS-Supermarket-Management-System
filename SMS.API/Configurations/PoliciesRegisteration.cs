using Microsoft.AspNetCore.Authorization;
using SMS.API.Authorization.Requirements;
using SMS.API.Helpers.Constants;

namespace SMS.API.Configurations
{
    public static class PoliciesRegisteration
    {
        public static void AddUserOwnerOrAdminPolicy(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy(PoliciesNames.UserOwnerOrAdmin, policy =>
                {
                    policy.Requirements.Add(new UserOwnerOrAdminRequirement());
                });
            });

            services.AddSingleton<IAuthorizationHandler, Authorization.Handlers.UserOwnerOrAdminHandler>();
        }
    }
}

using Microsoft.AspNetCore.Authorization;

namespace SMS.API.Authorization.Requirements;

public class UserOwnerOrAdminRequirement : IAuthorizationRequirement
{
}
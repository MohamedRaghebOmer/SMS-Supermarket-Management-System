using SMS.API.Interfaces;
using SMS.Shared.Enums;
using System.Text.Json;

namespace SMS.API.Helpers
{
    public sealed class AttemptedUsernameResolver : IAttemptedUsernameResolver
    {
        public async Task<string?> ResolveAsync(HttpContext context, AuditActionType actionType)
        {
            if (actionType is not (AuditActionType.Login
                or AuditActionType.FailedLogin
                or AuditActionType.Register))
            {
                return null;
            }

            context.Request.EnableBuffering();

            context.Request.Body.Position = 0;

            using var reader = new StreamReader(
                context.Request.Body,
                leaveOpen: true);

            var body = await reader.ReadToEndAsync();

            context.Request.Body.Position = 0;

            if (string.IsNullOrWhiteSpace(body))
                return null;

            using var doc = JsonDocument.Parse(body);

            if (doc.RootElement.TryGetProperty("username", out var usernameProp))
                return usernameProp.GetString();

            if (doc.RootElement.TryGetProperty("Username", out var usernameProp2))
                return usernameProp2.GetString();

            if (doc.RootElement.TryGetProperty("email", out var emailProp))
                return emailProp.GetString();

            if (doc.RootElement.TryGetProperty("Email", out var emailProp2))
                return emailProp2.GetString();

            return null;
        }
    }
}

using Microsoft.Net.Http.Headers;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace SMS.API.Helpers
{
    public static class AuditLogHelper
    {
        public static int GetUserId(HttpContext context)
        {
            if (context.User.Identity?.IsAuthenticated == true)
            {
                var userIdClaim = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                if (userIdClaim is not null && int.TryParse(userIdClaim.Value, out int userId))
                {
                    return userId;
                }
            }
            return 0; // Return 0 for unauthenticated users or if the claim is missing/invalid
        }

        public static string GetEndpoint(HttpContext context)
        {
            return context.Request.Path.ToString();
        }

        public static Guid GetOrCreateCorrelationId(HttpContext context)
        {
            const string correlationIdHeaderFormat = "X-Correlation-ID";

            Guid correlationId;

            bool hasCorrelationId =
                context.Request.Headers.TryGetValue(
                    correlationIdHeaderFormat,
                    out var correlationIdHeader);

            bool isValidGuid =
                Guid.TryParse(
                    correlationIdHeader,
                    out var parsedCorrelationId);

            if (hasCorrelationId && isValidGuid)
            {
                correlationId = parsedCorrelationId;
            }
            else
            {
                correlationId = Guid.NewGuid();
            }

            context.Items["CorrelationId"] = correlationId;

            context.Response.Headers[correlationIdHeaderFormat] =
                correlationId.ToString();

            return correlationId;
        }

        public static async Task<string> GetRequestBodyAsync(HttpContext context)
        {
            const int maxBodySize = 1024 * 50;

            if (context.Request.ContentLength > maxBodySize)
            {
                return "[Request body too large]";
            }

            if (context.Request.ContentType?.Contains("application/json") != true)
            {
                return "[Unsupported content type]";
            }

            context.Request.EnableBuffering();

            context.Request.Body.Position = 0;

            using var reader = new StreamReader(
                context.Request.Body,
                encoding: Encoding.UTF8,
                detectEncodingFromByteOrderMarks: false,
                leaveOpen: true);

            var body = await reader.ReadToEndAsync();

            context.Request.Body.Position = 0;

            return MaskSensitiveData(body);
        }

        private static string MaskSensitiveData(string body)
        {
            if (string.IsNullOrWhiteSpace(body))
            {
                return body;
            }

            try
            {
                JsonNode? jsonNode = JsonNode.Parse(body);

                if (jsonNode is null)
                {
                    return body;
                }

                string[] sensitiveFields =
                [
                    "password",
                "confirmPassword",
                "token",
                "refreshToken",
                "accessToken",
                "apiKey",
                "secret",
                "creditCard",
                "cvv"
                ];

                MaskNode(jsonNode, sensitiveFields);

                return jsonNode.ToJsonString(new JsonSerializerOptions
                {
                    WriteIndented = false
                });
            }
            catch
            {
                // If body is not valid JSON, return it as-is
                return body;
            }
        }

        private static void MaskNode(JsonNode node, string[] sensitiveFields)
        {
            if (node is JsonObject jsonObject)
            {
                foreach (var property in jsonObject.ToList())
                {
                    if (property.Value is null)
                    {
                        continue;
                    }

                    bool isSensitive =
                        sensitiveFields.Any(field =>
                            field.Equals(property.Key, StringComparison.OrdinalIgnoreCase));

                    if (isSensitive)
                    {
                        jsonObject[property.Key] = "***";
                    }
                    else
                    {
                        MaskNode(property.Value, sensitiveFields);
                    }
                }
            }
            else if (node is JsonArray jsonArray)
            {
                foreach (var item in jsonArray)
                {
                    if (item is not null)
                    {
                        MaskNode(item, sensitiveFields);
                    }
                }
            }
        }

        public static string? GetUserAgent(HttpContext context)
        {
            return context.Request.Headers[HeaderNames.UserAgent].ToString() ?? null;
        }

        public static HttpStatusCode GetStatusCode(HttpContext context)
        {
            return (HttpStatusCode)context.Response.StatusCode;
        }

        public static string GetIpAddress(HttpContext context)
        {
            return context.Connection.RemoteIpAddress?.ToString() ?? string.Empty;
        }
    }
}
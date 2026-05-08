namespace SMS.API.Middlewares
{
    public sealed class CorrelationIdMiddleware
    {
        private const string HeaderName = "X-Correlation-Id";
        private readonly RequestDelegate _next;

        public CorrelationIdMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            string correlationId;

            if (context.Request.Headers.TryGetValue(
                HeaderName,
                out var existingCorrelationId) &&
                Guid.TryParse(existingCorrelationId, out var parsedCorrelationId))
            {
                correlationId = parsedCorrelationId.ToString();
            }
            else
            {
                correlationId = Guid.NewGuid().ToString();
            }

            context.Items["CorrelationId"] = correlationId;

            context.Response.Headers[HeaderName] = correlationId;

            await _next(context);
        }
    }
}
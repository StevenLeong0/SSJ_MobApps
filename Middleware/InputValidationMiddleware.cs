using SeniorLearnApi.DTOs.Responses;
using System.Text.Json;

namespace SeniorLearnApi.Middleware;

public class InputValidationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<InputValidationMiddleware> _logger;

    public InputValidationMiddleware(RequestDelegate next, ILogger<InputValidationMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if ((context.Request.Method == "POST" || context.Request.Method == "PUT")
            && context.Request.ContentLength > 0)
        {
            var contentType = context.Request.ContentType;
            if (string.IsNullOrEmpty(contentType) || !contentType.Contains("application/json", StringComparison.OrdinalIgnoreCase))
            {
                await HandleValidationError(context, "Content-Type must be application/json.", 415); // Unsupported Media Type
                return;
            }
        }

        if (context.Request.ContentLength > 10 * 1024 * 1024)
        {
            await HandleValidationError(context, "Request body too large.", 413); // Payload Too Large
            return;
        }

        if (!context.Request.Headers.ContainsKey("User-Agent"))
        {
            _logger.LogWarning("Request missing User-Agent header from {RemoteIpAddress}",
                context.Connection.RemoteIpAddress);
        }

        foreach (var query in context.Request.Query)
        {
            foreach (var value in query.Value)
            {
                if (ContainsSuspiciousContent(value))
                {
                    await HandleValidationError(context, "Invalid input detected.", 400); // Bad Request
                    return;
                }
            }
        }

        await _next(context);
    }

    private static bool ContainsSuspiciousContent(string input)
    {
        var suspiciousPatterns = new[]
        {
            "<script", "javascript:", "onload=", "onerror=", "onclick=", "onmouseover=",
            "SELECT", "INSERT", "UPDATE", "DELETE", "DROP", "UNION", "EXEC", "EXECUTE"
        };

        return suspiciousPatterns.Any(pattern =>
            input.Contains(pattern, StringComparison.OrdinalIgnoreCase));
    }

    private static async Task HandleValidationError(HttpContext context, string message, int statusCode = 400)
    {
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";

        var response = ApiResponse<object>.ErrorResponse(message);
        var jsonResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await context.Response.WriteAsync(jsonResponse);
    }
}

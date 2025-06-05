using SeniorLearnApi.DTOs.Responses;
using System.Net;
using System.Text.Json;

namespace SeniorLearnApi.Middleware;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred. RequestPath: {RequestPath}, Method: {Method}",
                context.Request.Path, context.Request.Method);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var response = exception switch
        {
            ArgumentException => new { StatusCode = (int)HttpStatusCode.BadRequest, Message = "Invalid request data" },
            UnauthorizedAccessException => new { StatusCode = (int)HttpStatusCode.Unauthorized, Message = "Unauthorized" },
            KeyNotFoundException => new { StatusCode = (int)HttpStatusCode.NotFound, Message = "Resource not found" },
            InvalidOperationException => new { StatusCode = (int)HttpStatusCode.BadRequest, Message = exception.Message },
            TimeoutException => new { StatusCode = (int)HttpStatusCode.RequestTimeout, Message = "Request timeout" },
            _ => new { StatusCode = (int)HttpStatusCode.InternalServerError, Message = "An error occurred while processing your request" }
        };

        context.Response.StatusCode = response.StatusCode;

        var apiResponse = ApiResponse<object>.ErrorResponse(response.Message);
        var jsonResponse = JsonSerializer.Serialize(apiResponse, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await context.Response.WriteAsync(jsonResponse);
    }
}

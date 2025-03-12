
using CleanArchitecture.Shared.Models.Response;

namespace CleanArchitecture.Web.Middlewares;

public class GlobalExceptionMiddleware(ILoggerFactory logger) : IMiddleware
{
    private readonly ILogger _logger = logger.CreateLogger<GlobalExceptionMiddleware>();
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception occurred.");
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";

            var response = ApiResponse<string>.Fail("An unexpected error occurred.");
            await context.Response.WriteAsJsonAsync(response);
        }
    }
}

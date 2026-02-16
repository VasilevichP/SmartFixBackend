using SmartFix.Domain.Exceptions;

namespace SmartFix;

public class CustomExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<CustomExceptionHandlerMiddleware> _logger;

    public CustomExceptionHandlerMiddleware(RequestDelegate next, ILogger<CustomExceptionHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (HttpException ex)
        {
            context.Response.StatusCode = (int)ex.StatusCode;
            context.Response.ContentType = "application/json";

            var response = new {message = ex.Message};
            await context.Response.WriteAsJsonAsync(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error occurred");

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";

            var response = new {message = "Внутренняя ошибка сервера"};
            await context.Response.WriteAsJsonAsync(response);
        }
    }
}
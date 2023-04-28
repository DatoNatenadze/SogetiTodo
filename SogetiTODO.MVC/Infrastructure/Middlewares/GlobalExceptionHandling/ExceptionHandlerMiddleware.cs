using Serilog;

namespace SogetiTODO.Infrastructure.Middlewares.GlobalExceptionHandling;

public class ExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next.Invoke(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        var error = new ApiError(context, ex);
        Log.Error(ex, "Exception: {Error}", error);
        context.Response.Clear();
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = error.Status.Value;
        context.Response.Redirect($"/Error/Index?errorCode={error.Code}");
        return Task.CompletedTask;
    }
}
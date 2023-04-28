using System.Net;
using Microsoft.AspNetCore.Mvc;
using SogetiTODO.Services.Exceptions;

namespace SogetiTODO.Infrastructure.Middlewares.GlobalExceptionHandling;

public sealed class ApiError : ProblemDetails
{
    public const string UnhandledError = "UnhandledException";

    public ApiError(HttpContext context, Exception exception)
    {
        TraceId = context.TraceIdentifier;
        Code = UnhandledError;
        Title = exception.Message;
        LogLevel = LogLevel.None;
        Instance = context.Request.Path;
        HandleException((dynamic)exception);
    }

    public LogLevel LogLevel { get; set; }
    public string Code { get; set; }

    private string TraceId
    {
        get
        {
            if (Extensions.TryGetValue("TraceId", out var traceId)) return (string)traceId;

            return null;
        }
        set => Extensions["TraceId"] = value;
    }

    private void HandleException(Exception exception)
    {
        Code = UnhandledError;
        Status = (int)HttpStatusCode.InternalServerError;
        Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1";
        Title = exception.Message;
        LogLevel = LogLevel.Error;
    }

    private void HandleException(TodoNotFoundException exception)
    {
        Code = exception.Code;
        Status = (int)HttpStatusCode.NotFound;
        Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.4";
        Title = exception.Message;
        LogLevel = LogLevel.Error;
    }

    private void HandleException(TodoAlreadyInStateException exception)
    {
        Code = exception.Code;
        Status = (int)HttpStatusCode.Conflict;
        Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.8";
        Title = exception.Message;
        LogLevel = LogLevel.Error;
    }
}
using Microsoft.AspNetCore.Http;

namespace SogetiTODO.Services.Exceptions;

public class TodoAlreadyInStateException : Exception
{
    public readonly string Code = StatusCodes.Status409Conflict.ToString();

    public TodoAlreadyInStateException(string message) : base(message)
    {
    }
}
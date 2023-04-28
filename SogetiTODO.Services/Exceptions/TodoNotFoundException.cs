using Microsoft.AspNetCore.Http;

namespace SogetiTODO.Services.Exceptions;

public class TodoNotFoundException : Exception
{
    public readonly string Code = StatusCodes.Status404NotFound.ToString();

    public TodoNotFoundException(string message) : base(message)
    {
    }
}
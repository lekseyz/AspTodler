using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Misc;

public record Error(int Code, string Message)
{
    public ObjectResult GetObjectResult()
    {
        return new ObjectResult(new {message = Message})
        {
            StatusCode = Code
        };
    }
    
    public static Error NotFound(string message = "Object not found") => new Error(StatusCodes.Status404NotFound, message);
    public static Error WrongValue(string message = "Wrong value") => new Error(StatusCodes.Status400BadRequest, message);
    public static Error AuthError(string message = "Auth error") => new Error(StatusCodes.Status401Unauthorized, message);
}
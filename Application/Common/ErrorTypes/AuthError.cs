namespace Application.Common.ErrorTypes;

public record AuthError(string Message) : Error(Message);
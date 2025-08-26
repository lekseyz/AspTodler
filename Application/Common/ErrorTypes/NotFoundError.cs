namespace Application.Common.ErrorTypes;

public record NotFoundError(string Message) : Error(Message);
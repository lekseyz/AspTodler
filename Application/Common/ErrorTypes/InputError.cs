namespace Application.Common.ErrorTypes;

public record InputError(string Message) : Error(Message);
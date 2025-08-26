namespace Domain.Exceptions;

public class InvalidNoteOperation : Exception
{
    public InvalidNoteOperation(string message) : base(message) { }
    public InvalidNoteOperation() { }
}
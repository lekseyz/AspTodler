namespace Domain.ValueTypes;

public class NoteContent
{
    public required string Content { get; init; }

    private NoteContent( string content)
    {
        Content = content;
    }
}
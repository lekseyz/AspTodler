namespace Domain.ValueTypes;

public class NoteContent
{
    public string Content { get; private set; }

    public NoteContent( string content)
    {
        Content = content;
    }
}
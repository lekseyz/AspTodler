namespace Domain.ValueTypes;

public class NoteContent
{
    public string Content { get; set; }

    public NoteContent( string content)
    {
        Content = content;
    }
}
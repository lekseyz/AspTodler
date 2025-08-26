namespace Domain.Models;

public class NoteContent
{
    public Guid Id { get; private set; }
    public string Content { get; private set; }

    private NoteContent(Guid id, string content)
    {
        Id = id;
        Content = content;
    }

    public static NoteContent Create()
    {
        return new NoteContent(Guid.NewGuid(), string.Empty);
    }

    public static NoteContent Construct(Guid id, string content)
    {
        return new NoteContent(id, content);
    }
}
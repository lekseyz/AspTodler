namespace Domain.Models;

public class NoteInfo
{
    public string Title { get; private set; }
    public Guid CreatorId { get; private set; }
    public DateTime Created { get; private set; }
    public DateTime LastModified { get; private set; }

    public NoteInfo(string title, Guid creatorId, DateTime created,  DateTime lastModified)
    {
        Title = title;
        CreatorId = creatorId;
        Created = created;
        LastModified = lastModified;
    }
}
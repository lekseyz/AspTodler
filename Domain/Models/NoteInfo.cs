namespace Domain.Models;

public class NoteInfo
{
    public Guid Id { get; private set; }
    public string Title { get; private set; }
    public Guid CreatorId { get; private set; }
    public DateTime Created { get; private set; }
    public DateTime LastModified { get; private set; }

    private NoteInfo(Guid id, string title, Guid creatorId, DateTime created,  DateTime lastModified)
    {
        Id = id;
        Title = title;
        CreatorId = creatorId;
        Created = created;
        LastModified = lastModified;
    }

    public static NoteInfo Construct(Guid id, string title, Guid creatorId, DateTime created, DateTime lastModified)
    {
        return new NoteInfo(id,  title, creatorId, created, lastModified);
    }

    public static NoteInfo Create(string title, Guid creatorId)
    {
        var creationTime = DateTime.UtcNow;
        return new NoteInfo(Guid.NewGuid(), title, creatorId, creationTime, creationTime);
    }
}
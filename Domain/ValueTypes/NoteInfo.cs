namespace Domain.Models;

public class NoteInfo
{
    public required string Title { get; init; }
    public required Guid CreatorId { get; init; }
    public required DateTime Created { get; init; }
    public required DateTime LastModified { get; init; }

    private NoteInfo(string title, Guid creatorId, DateTime created,  DateTime lastModified)
    {
        Title = title;
        CreatorId = creatorId;
        Created = created;
        LastModified = lastModified;
    }
}
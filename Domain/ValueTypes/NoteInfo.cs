namespace Domain.Models;

public class NoteInfo
{
    public string Title { get; set; }
    public Guid CreatorId { get; set; }
    public DateTime Created { get; set; }
    public DateTime LastModified { get; set; }

    public NoteInfo(string title, Guid creatorId, DateTime created,  DateTime lastModified)
    {
        Title = title;
        CreatorId = creatorId;
        Created = created;
        LastModified = lastModified;
    }
}
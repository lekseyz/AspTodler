namespace Persistence.DbEntities;

public class NoteInfoEntity
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public Guid CreatorId { get; set; }
    public UserEntity Creator { get; set; }
    public DateTime Created { get; set; }
    public DateTime LastModified { get; set; }
}
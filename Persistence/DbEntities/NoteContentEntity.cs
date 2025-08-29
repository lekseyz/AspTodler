using Domain.Models;

namespace Persistence.DbEntities;

public class NoteContentEntity
{
    public Guid Id { get; set; }
    public NoteInfoEntity Info { get; set; }
    public string Content { get; set; }
}
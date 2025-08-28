using Domain.Exceptions;
using Domain.ValueTypes;

namespace Domain.Models;

public class Note
{
    public enum Types
    {
        InfoOnly,
        Full
    }
    
    private readonly NoteContent? _noteContent;
    
    public Guid Id { get; private set; }
    public Types Type { get; private set; }
    public NoteInfo Info { get; private set; }

    public NoteContent Content
    {
        get
        {
            return Type switch
            {
                Types.Full     => _noteContent,
                Types.InfoOnly     => throw new InvalidNoteOperation("Trying get content from info type entity")
            };
        }
        
    }

    private Note(Guid id, Types type, NoteInfo info, NoteContent? content = null)
    {
        Id = id;
        Type = type;
        Info = info;
        _noteContent = content;
    }

    public static Note Create(Guid id, NoteInfo info)
    {
        NoteContent content = new(String.Empty);
        return new (Guid.NewGuid(), Types.InfoOnly, info, content);
    }
    public static Note Create(Guid id, NoteInfo info, NoteContent content)
    {
        return new (Guid.NewGuid(), Types.InfoOnly, info, content);
    }

    public static Note ConstructInfo(Guid id, NoteInfo info)
    {
        return new (id,  Types.InfoOnly, info);
    }

    public static Note Construct(Guid id, NoteInfo info, NoteContent content)
    {
        return new (id, Types.Full, info, content);
    }
}
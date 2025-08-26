using Domain.Exceptions;
using Domain.ValueTypes;

namespace Domain.Models;

public class Note
{
    public enum Types
    {
        Info,
        Content,
        Empty,
        Full
    }
    
    private NoteInfo? _noteInfo;
    private NoteContent? _noteContent;
    
    public Guid Id { get; private set; }
    public Types Type { get; private set; }
    public NoteInfo Info
    {
        get
        {
            return Type switch
            {
                Types.Info     => _noteInfo,
                Types.Full     => _noteInfo,
                Types.Content  => throw new InvalidNoteOperation("Trying get info from content type entity"),
                Types.Empty    => throw new InvalidNoteOperation("Trying get info from empty note")
            };
        }
    }

    public NoteContent Content
    {
        get
        {
            return Type switch
            {
                Types.Content  => _noteContent,
                Types.Full     => _noteContent,
                Types.Info     => throw new InvalidNoteOperation("Trying get content from info type entity"),
                Types.Empty    => throw new InvalidNoteOperation("Trying get content from empty note")
            };
        }
        
    }

    private Note(Guid id, Types type, NoteInfo? info = null, NoteContent? content = null)
    {
        Id = id;
        Type = type;
        _noteInfo = info;
        _noteContent = content;
    }

    public static Note CreateContent(NoteContent content)
    {
        return new Note(id: Guid.NewGuid(), type: Types.Content, content: content);
    }
    public static Note CreateInfo(NoteInfo info)
    {
        return new Note(id: Guid.NewGuid(), type: Types.Info, info: info);
    }
    public static Note CreateFull(NoteInfo info, NoteContent content)
    {
        return new Note(id: Guid.NewGuid(), type: Types.Full, info: info, content: content);
    }
    public static Note ConstructContent(Guid id, NoteContent content)
    {
        return new Note(id: id, type: Types.Content, content: content);
    }
    public static Note ConstructInfo(Guid id, NoteInfo info)
    {
        return new Note(id: id, type: Types.Content, info: info);
    }
    public static Note ConstructFull(Guid id, NoteInfo info, NoteContent content)
    {
        return new Note(id: id, type: Types.Content, info: info, content: content);
    }
}
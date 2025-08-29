using Domain.Models;
using Domain.ValueTypes;
using Microsoft.EntityFrameworkCore;
using Persistence.DbEntities;

namespace Persistence.Repositories;

public class NoteRepository
{
    private record InfoAndContent(NoteInfoEntity NoteInfo, NoteContentEntity NoteContent);
    
    private readonly TodlerDbContext _context;

    public NoteRepository(TodlerDbContext context)
    {
        _context = context;
    }

    public async Task Create(Note note)
    {
        var userId = note.Info.CreatorId;
        
        //TODO: repos helpers with basic methods as get user by id
        
        var info = MapFromInfo(note, userId);
        var content = MapFromContent(note);
        
        await _context.NoteInfos.AddAsync(info);
        await _context.NoteContents.AddAsync(content);
        
        await _context.SaveChangesAsync();
    }

    public async Task<List<Note>> GetUserNotes(Guid userId, Note.Types type)
    {
        return type switch
        {
            Note.Types.InfoOnly => await _context.NoteInfos
                .AsNoTracking()
                .Where(n => n.CreatorId == userId)
                .Select(n => Note.ConstructInfo(n.Id, new NoteInfo(n.Title, n.CreatorId, n.Created, n.LastModified)))
                .ToListAsync(),
            Note.Types.Full => await JoinInfoAndContext(_context.NoteInfos.AsNoTracking(), _context.NoteContents.AsNoTracking())
                .Where(infoAndContent =>  infoAndContent.NoteInfo.CreatorId == userId)
                .Select(iac => Note.Construct(iac.NoteInfo.Id, MapInfo(iac.NoteInfo), MapContent(iac.NoteContent)))
                .ToListAsync(),
            { } => throw new NotImplementedException()
        };
    }

    public async Task<Note?> GetNote(Guid id)
    {
        var infoAndContent = await JoinInfoAndContext(_context.NoteInfos.AsNoTracking(), _context.NoteContents.AsNoTracking())
            .FirstOrDefaultAsync(s => s.NoteInfo.Id == id);
        if (infoAndContent is null)
            return null;
        
        var (info, content) = infoAndContent;
        
        return Note.Construct(info.Id, MapInfo(info), MapContent(content));
    }

    public async Task UpdateNote(Note note)
    {
        var infoAndContent = await JoinInfoAndContext(_context.NoteInfos, _context.NoteContents)
            .FirstOrDefaultAsync(s => s.NoteInfo.Id == note.Id);
        
        var (info, content) = infoAndContent!;
        
        info.Title = note.Info.Title;
        info.LastModified = note.Info.LastModified;
        content.Content = note.Content.Content;
        await _context.SaveChangesAsync();
    }

    public async Task<bool> DeleteNote(Guid id)
    {
        var infoAndContent = await JoinInfoAndContext(_context.NoteInfos, _context.NoteContents)
            .FirstOrDefaultAsync(s => s.NoteInfo.Id == id);
        if (infoAndContent is null)
            return false;
        
        var (info, content) = infoAndContent;
        _context.NoteInfos.Remove(info);
        _context.NoteContents.Remove(content);
        
        await _context.SaveChangesAsync();
        return true;
    }

    private IQueryable<InfoAndContent> JoinInfoAndContext(IQueryable<NoteInfoEntity> noteInfos, IQueryable<NoteContentEntity> noteContents)
    {
        return noteInfos
            .Join(noteContents, 
                info => info.Id, 
                content => content.Id, 
                (info, content) => new InfoAndContent(info, content)
                );
    }

    private NoteInfo MapInfo(NoteInfoEntity entity)
    {
        return new NoteInfo(entity.Title, entity.CreatorId, entity.Created, entity.LastModified);
    }

    private NoteContent MapContent(NoteContentEntity entity)
    {
        return new NoteContent(entity.Content);
    }

    private NoteInfoEntity MapFromInfo(Note note, Guid userId)
    {
        var info = note.Info;
        return new NoteInfoEntity()
        {
            Title = info.Title,
            Id = note.Id,
            Created = info.Created,
            CreatorId = userId,
            LastModified = info.LastModified
        };
    }
    
    private NoteContentEntity MapFromContent(Note note)
    {
        var content = note.Content;
        return new NoteContentEntity()
        {
            Id = note.Id,
            Content = content.Content
        };
    }
}
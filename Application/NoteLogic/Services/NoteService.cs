using Application.Common;
using Application.Common.ErrorTypes;
using Application.NoteLogic.Contracts;
using Application.NoteLogic.Interfaces;
using Domain.Models;
using Persistence.Repositories;

namespace Application.NoteLogic.Services;

public class NoteService : INoteService
{
    private readonly NoteRepository _noteRepository;

    public NoteService(NoteRepository noteRepository)
    {
        _noteRepository = noteRepository;
    }

    public async Task<ResultOptional<Note>> Create(NoteCreationRequest request)
    {
        var noteInfo = new NoteInfo(request.Title, request.UserId, DateTime.UtcNow, DateTime.UtcNow);
        var note = Note.Create(noteInfo);
        
        await _noteRepository.Create(note);
        return ResultOptional<Note>.Success(note);
    }
    
    public async Task<List<Note>> GetNotes(Guid userId)
    {
        return await _noteRepository.GetUserNotes(userId, Note.Types.InfoOnly);
    }

    public async Task<ResultOptional<Note>> Get(Guid id)
    {
        var note =  await _noteRepository.GetNote(id);
        if (note == null)
            return ResultOptional<Note>.Failure(new NotFoundError($"Not found note with id = {id}"));
        
        return ResultOptional<Note>.Success(note);
    }

    public async Task<Result> Update(Guid id, string? title, string? content)
    {
        var note = await _noteRepository.GetNote(id);
        if (note is null) 
            return Result.Failure(new NotFoundError($"Note with id = {id} not found"));

        if (title is not null)
            note.UpdateTitle(title);
        
        if (content is not null) 
            note.UpdateContent(content);
        
        await _noteRepository.UpdateNote(note);
        
        return Result.Success();
    }

    public async Task<Result> Delete(Guid id)
    {
        if (await _noteRepository.DeleteNote(id))
        {
            return Result.Success();
        }

        return Result.Failure(new NotFoundError($"Note with id = {id} not found"));
    }
}

using Application.Common;
using Application.NoteLogic.Contracts;
using Domain.Models;

namespace Application.NoteLogic.Interfaces;

public interface INoteService
{
    Task<ResultOptional<Note>> Create(NoteCreationRequest request);
    Task<List<Note>> GetNotes(Guid userId);
    Task<ResultOptional<Note>> Get(Guid id);
    Task<Result> Update(Guid id, string? title, string? content);
    Task<Result> Delete(Guid id);
}
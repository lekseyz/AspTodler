using System.Security.Claims;
using Application.Common.ErrorTypes;
using Application.NoteLogic.Contracts;
using Application.NoteLogic.Interfaces;
using Domain.Models;
using Domain.ValueTypes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Dtos;

namespace Presentation.Controllers;

[ApiController]
[Route("api/notes")]
public class NoteController : ControllerBase
{
    private readonly INoteService _noteService;

    public NoteController(INoteService noteService)
    {
        _noteService = noteService;
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<List<NoteResponse>>> Get()
    {
        var userId = GetUserId();
        var notes = await _noteService.GetNotes(userId);

        return notes.Select(MapToNoteResponse).ToList();
    }

    [HttpGet("{id:guid}")]
    [Authorize]
    public async Task<ActionResult<NoteContentResponse>> GetNote([FromRoute] Guid id)
    {
        var note = await _noteService.Get(id);

        if (note.IsFailure)
        {
            var error = note.GetError;

            return error switch
            {
                NotFoundError notFoundError => NotFound(error.Message),
                { } defaultError => Problem(detail: defaultError.Message, statusCode: 400,
                    title: $"get note {id} error")
            };
        }

        return MapToNoteContentResponse(note.Value!.Content);
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<NoteResponse>> Create()
    {
        var userId = GetUserId();
        var noteResult = await _noteService.Create(new NoteCreationRequest(userId, "New Note"));

        if (noteResult.IsFailure)
        {
            return Problem(statusCode: 500, title: "Create note error");
        }
        
        return MapToNoteResponse(noteResult.Value!);
    }

    [HttpPut("{id:guid}")]
    [Authorize]
    public async Task<ActionResult> Update([FromRoute] Guid id, [FromBody] NoteUpdateRequest request)
    {
        var updateResult = await _noteService.Update(id, request.Title, request.Content);
        if (updateResult.IsFailure)
        {
            var error = updateResult.GetError;
            return error switch
            {
                NotFoundError notFoundError => NotFound(notFoundError.Message),
                { } baseError => Problem(title: "Update note error", statusCode: 400, detail: baseError.Message)
            };
        }
        return Ok();
    }

    [HttpDelete("{id:guid}")]
    [Authorize]
    public async Task<ActionResult> Delete([FromRoute] Guid id)
    {
        var deleteResult = await _noteService.Delete(id);
        if (deleteResult.IsFailure)
        {
            var error = deleteResult.GetError;
            return error switch
            {
                NotFoundError notFoundError => NotFound(notFoundError.Message),
                { } baseError => Problem(title: "Update note error", statusCode: 400, detail: baseError.Message)
            };
        }
        
        return Ok();
    }

    private Guid GetUserId()
    {
        return Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    }

private NoteResponse MapToNoteResponse(Note note)
    {
        return new NoteResponse(note.Id, note.Info.Title, note.Info.Created, note.Info.LastModified);
    }

    private NoteContentResponse MapToNoteContentResponse(NoteContent noteContent)
    {
        return new NoteContentResponse(noteContent.Content);
    }
}
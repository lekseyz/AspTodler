namespace Application.NoteLogic.Contracts;

public record NoteCreationRequest(Guid UserId, string Title);
namespace Presentation.Dtos;

public record NoteResponse(Guid Id, string Title, DateTime Created, DateTime Modified);
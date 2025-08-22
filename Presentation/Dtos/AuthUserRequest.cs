using System.ComponentModel.DataAnnotations;

namespace Presentation.Dtos;

public record AuthUserRequest([EmailAddress, Required] string Email, [Required] string Password);
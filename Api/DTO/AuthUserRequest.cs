using System.ComponentModel.DataAnnotations;

namespace AspTodler.DTO;

public record AuthUserRequest([EmailAddress, Required] string Email, [Required] string Password);
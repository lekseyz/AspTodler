using Domain.Models;

namespace Application.User.Contracts;

public record AuthResult(UserModel User, string Token, string RefreshToken);
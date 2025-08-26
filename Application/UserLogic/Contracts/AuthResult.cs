using Domain.Models;

namespace Application.UserLogic.Contracts;

public record AuthResult(User User, string Token, string RefreshToken);
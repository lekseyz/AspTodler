namespace Domain.Models;

public class UserModel
{
    public Guid Id { get; private set; }
    public string Email { get; private set; }
    public string PasswordHash { get; private set; }
    public string PasswordSalt { get; private set; }
    public string RefreshToken { get; private set; }

    public UserModel(Guid id, string email, string passwordHash, string passwordSalt, string refreshToken)
    {
        Id = id;
        Email = email;
        PasswordHash = passwordHash;
        PasswordSalt = passwordSalt;
        RefreshToken = refreshToken;
    }
}
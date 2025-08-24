namespace Infrastructure.DbEntities;

public class UserEntity
{
    public int Id { get; set; }
    public Guid Guid { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public string PasswordSalt { get; set; }
    public List<RefreshTokenEntity> RefreshTokens { get; set; } = [];
}
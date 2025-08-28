namespace Persistence.DbEntities;

public class UserEntity
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public string PasswordSalt { get; set; }
    public List<RefreshTokenEntity> RefreshTokens { get; set; } = [];
    public List<NoteInfoEntity> Notes { get; set; } = [];
}
using Domain.Models;

namespace Infrastructure.DbEntities;

public class RefreshTokenEntity
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public UserEntity User { get; set; }
    public string Token { get; set; }
    public DateTime Expires { get; set; }
    public bool IsActive { get; set; } = true;
    public int? NextTokenId { get; set; } = null;
    public RefreshTokenEntity? NextToken { get; set; } = null;

    public static RefreshTokenEntity Create(UserEntity user, RefreshToken token)
    {
        return new RefreshTokenEntity()
        {
            User = user,
            Token = token.Token,
            Expires = token.Expires
        };
    }
}
namespace Infrastructure.DbEntities;

public class RefreshToken
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
    public string Token { get; set; }
    public DateTime Expires { get; set; }
    public bool IsActive { get; set; } = true;
    public int? NextTokenId { get; set; } = null;
    public RefreshToken? NextToken { get; set; } = null;
}
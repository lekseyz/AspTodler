namespace Infrastructure.Options;

public class AuthOptions
{
    public string SecretKey { get; init; } = string.Empty;
    public int TokenLifetimeHours { get; init; }
    public int RefreshLength { get; init; }
    public int RefreshLifetimeDays { get; init; }
}
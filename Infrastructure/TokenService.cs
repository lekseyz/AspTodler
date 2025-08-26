using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Domain.Models;
using Domain.ValueTypes;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Infrastructure.Options;

namespace Infrastructure;

public class TokenService
{
    private readonly AuthOptions _options;

    public TokenService(IOptions<AuthOptions> options)
    {
        _options = options.Value;
    }
    
    public string GenerateToken(User user)
    {
        Claim[] claims = [new("id", user.Id.ToString()), new("email",  user.Email)];
        
        var signingCredentials = new SigningCredentials(
            key: new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey)),
            algorithm: SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            claims: claims,
            signingCredentials: signingCredentials,
            expires: DateTime.UtcNow.AddHours(_options.TokenLifetimeHours)
        );
        
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public RefreshToken GenerateRefreshToken()
    {
        string tokenChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        Random random = new Random();
        char[] refreshToken = new char[_options.RefreshLength]; 

        // TODO refresh token lifetime
        for (int i = 0; i < _options.RefreshLength; i++)
        {
            refreshToken[i] = tokenChars[random.Next(tokenChars.Length)];
        }

        return new RefreshToken(refreshToken.ToString()!, DateTime.UtcNow.AddDays(_options.RefreshLifetimeDays));
    }
}
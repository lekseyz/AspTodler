using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Infrastructure.Options;

namespace Presentation.Extensions;

public static class AuthExtensions
{
    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services,
        IConfiguration configuration)
    {
        var authOptions = configuration.GetSection("AuthOptions").Get<AuthOptions>();
        if (authOptions is null)
            throw new ArgumentNullException();
        
        services.Configure<AuthOptions>(configuration.GetSection(nameof(AuthOptions)));

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new()
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authOptions.SecretKey))
            };
        });
        
        services.AddAuthorization();
        
        return services;
    }
}
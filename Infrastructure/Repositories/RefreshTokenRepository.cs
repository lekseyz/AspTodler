using Domain.Models;
using Infrastructure.DbEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Misc;

namespace Infrastructure.Repositories;

public class RefreshTokenRepository
{
    private TodlerDbContext _context;
    private ILogger<RefreshTokenRepository> _logger;
    public RefreshTokenRepository(TodlerDbContext context, ILogger<RefreshTokenRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<bool> Check(Guid userGuid, string token)
    {
        return await _context.RefreshTokens.AsNoTracking()
            .AnyAsync(t =>
                t.Token == token &&
                t.User.Guid == userGuid &&
                t.IsActive &&
                t.Expires > DateTime.UtcNow
            );
    }

    public async Task<Result> Update(Guid userGuid, string oldToken, RefreshToken newToken)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Guid == userGuid);
        
        if (user == null) 
            return Result.Failure(Error.NotFound($"User {userGuid} not found"));
        
        var token = await _context.RefreshTokens
            .Include(t => t.User)
            .Where(t => t.Token == oldToken && t.UserId == user.Id)
            .FirstOrDefaultAsync(); 
        
        if (token == null) 
            return Result.Failure(Error.NotFound($"Token {oldToken} not found, cannot add new one"));

        var nextToken = RefreshTokenEntity.Create(token.User, newToken);
        await _context.RefreshTokens
            .AddAsync(nextToken);
        
        token.IsActive = false;
        token.NextToken = nextToken;
        
        await _context.SaveChangesAsync();
        
        return Result.Success();
    }

    public async Task<Result> Add(Guid userGuid, RefreshToken newToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Guid == userGuid);
        if (user == null) return Result.Failure(Error.NotFound($"User with guid {userGuid} not found"));
        
        await _context.RefreshTokens.AddAsync(new RefreshTokenEntity()
        {
            Expires = newToken.Expires,
            Token = newToken.Token,
            User = user,
        });
        await _context.SaveChangesAsync();
        
        return Result.Success();
    }
}